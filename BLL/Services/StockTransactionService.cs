using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.ProductDTOs;
using DAL.DTOs.StockDTOs;
using DAL.DTOs.TransactionDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private const int OutOfStockStatusId = 1;
        private const int AvailableProductStatusId = 3;
        private const string DefaultTransactionTypeName = "IN";

        private readonly IVariationRepository _variationRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly IStockVariationRepository _stockVariationRepository;
        private readonly IProductStatusRepository _productStatusRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StockTransactionService> _logger;
        private readonly UnleashedContext _context;

        public StockTransactionService(
            ITransactionRepository transactionRepository,
            IStockRepository stockRepository,
            IProductRepository productRepository,
            IVariationRepository variationRepository,
            IUserRepository userRepository,
            ITransactionTypeRepository transactionTypeRepository,
            IStockVariationRepository stockVariationRepository,
            IProductStatusRepository productStatusRepository,
            IProviderRepository providerRepository,
            IMapper mapper,
            ILogger<StockTransactionService> logger,
            UnleashedContext context)
        {
            _transactionRepository = transactionRepository;
            _stockRepository = stockRepository;
            _productRepository = productRepository;
            _variationRepository = variationRepository;
            _userRepository = userRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _stockVariationRepository = stockVariationRepository;
            _productStatusRepository = productStatusRepository;
            _providerRepository = providerRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task CreateStockTransactionsAsync(StockTransactionDTO stockTransactionDto)
        {
            if (stockTransactionDto == null) throw new ArgumentNullException(nameof(stockTransactionDto));
            if (stockTransactionDto.StockId == null || stockTransactionDto.ProviderId == null ||
                string.IsNullOrWhiteSpace(stockTransactionDto.Username) ||
                stockTransactionDto.Variations == null || !stockTransactionDto.Variations.Any())
            {
                throw new ArgumentException("Invalid StockTransactionDTO data provided for creation.");
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var dbTransaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var provider = await _providerRepository.GetByIdAsync(stockTransactionDto.ProviderId.Value)
                            ?? throw new KeyNotFoundException($"Provider not found with ID: {stockTransactionDto.ProviderId.Value}.");
                        var stock = await _stockRepository.GetByIdAsync(stockTransactionDto.StockId.Value)
                            ?? throw new KeyNotFoundException($"Stock not found with ID: {stockTransactionDto.StockId.Value}.");
                        var inchargeEmployee = await _userRepository.GetByUsernameAsync(stockTransactionDto.Username)
                            ?? throw new KeyNotFoundException($"Incharge employee not found with username: {stockTransactionDto.Username}.");
                        var transactionTypeName = stockTransactionDto.TransactionType ?? DefaultTransactionTypeName;
                        var transactionType = await _transactionTypeRepository.FindByNameAsync(transactionTypeName)
                            ?? throw new KeyNotFoundException($"TransactionType '{transactionTypeName}' not found.");

                        foreach (var variationQuantity in stockTransactionDto.Variations)
                        {
                            if (variationQuantity.ProductVariationId == null || variationQuantity.Quantity <= 0)
                            {
                                _logger.LogWarning("Invalid ProductVariationQuantity in DTO, skipping item.");
                                continue;
                            }
                            await UpdateStockAndLogTransactionAsync(
                                variationId: variationQuantity.ProductVariationId.Value,
                                stockId: stock.StockId,
                                quantityDelta: variationQuantity.Quantity.Value,
                                transactionType: transactionType,
                                inchargeEmployee: inchargeEmployee,
                                provider: provider
                            );
                        }

                        await _context.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during bulk stock transaction creation. Rolling back.");
                        await dbTransaction.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        private async Task UpdateStockAndLogTransactionAsync(int variationId, int stockId, int quantityDelta, TransactionType transactionType, User inchargeEmployee, Provider provider)
        {
            if (quantityDelta == 0) return;

            var variation = await _variationRepository.GetByIdAsync(variationId)
                ?? throw new KeyNotFoundException($"Variation not found with ID: {variationId}.");

            var stockVariation = await _stockVariationRepository.GetByIdAsync(variationId, stockId);

            if (quantityDelta < 0 && (stockVariation == null || stockVariation.StockQuantity < -quantityDelta))
            {
                throw new InvalidOperationException($"Insufficient stock for variation ID {variationId} in stock ID {stockId}.");
            }

            if (stockVariation != null)
            {
                stockVariation.StockQuantity = (stockVariation.StockQuantity ?? 0) + quantityDelta;
                await _stockVariationRepository.UpdateAsync(stockVariation);
            }
            else
            {
                stockVariation = new StockVariation
                {
                    StockId = stockId,
                    VariationId = variationId,
                    StockQuantity = quantityDelta
                };
                await _stockVariationRepository.AddAsync(stockVariation);
            }

            var transaction = new Transaction
            {
                StockId = stockId,
                VariationId = variationId,
                ProviderId = provider.ProviderId,
                InchargeEmployeeId = inchargeEmployee.UserId,
                TransactionTypeId = transactionType.TransactionTypeId,
                TransactionQuantity = quantityDelta,
                TransactionDate = DateOnly.FromDateTime(DateTime.UtcNow),
                TransactionProductPrice = variation.VariationPrice
            };
            await _transactionRepository.AddAsync(transaction);

            if (quantityDelta > 0)
            {
                if (variation.Product != null && variation.Product.ProductStatusId != AvailableProductStatusId)
                {
                    variation.Product.ProductStatusId = AvailableProductStatusId;
                }
            }
            else
            {
                var totalStock = await _stockVariationRepository.GetTotalStockQuantityForVariationAsync(variationId) ?? 0;
                if (totalStock <= 0)
                {
                    if (variation.Product != null)
                    {
                        variation.Product.ProductStatusId = OutOfStockStatusId;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a list of simplified transaction data transfer objects (DTOs)
        /// optimized for display in a list or card view (e.g., an Index page).
        /// </summary>
        public async Task<List<TransactionCardDTO>> GetAllTransactionCardsAsync()
        {
            try
            {
                // 1. Fetch simplified DTOs using a projection to be efficient.
                var simpleDtos = await _transactionRepository.FindSimplifiedTransactionCardDTOsOrderByIdDescAsync();

                if (simpleDtos == null || !simpleDtos.Any())
                {
                    return new List<TransactionCardDTO>();
                }

                // 2. Collect unique Product IDs to fetch their categories in a single query.
                var productIds = simpleDtos
                    .Where(dto => dto.ProductId != Guid.Empty)
                    .Select(dto => dto.ProductId)
                    .Distinct()
                    .ToList();

                // 3. Fetch Category names mapped by Product ID.
                var productCategoryMap = productIds.Any()
                    ? await _productRepository.GetCategoryNamesMapByProductIdsAsync(productIds)
                    : new Dictionary<Guid, List<string>>();

                // 4. Combine data into the final DTO.
                var finalDtos = new List<TransactionCardDTO>();
                foreach (var simpleDto in simpleDtos)
                {
                    productCategoryMap.TryGetValue(simpleDto.ProductId, out var categoryNames);
                    var categoryNameString = categoryNames != null && categoryNames.Any()
                        ? string.Join(", ", categoryNames)
                        : null; // Or "N/A" if preferred

                    var finalDto = new TransactionCardDTO
                    {
                        Id = simpleDto.TransactionId,
                        VariationImage = simpleDto.VariationImage,
                        ProductName = simpleDto.ProductName,
                        StockName = simpleDto.StockName,
                        TransactionTypeName = simpleDto.TransactionTypeName,
                        CategoryName = categoryNameString,
                        BrandName = simpleDto.BrandName,
                        SizeName = simpleDto.SizeName,
                        ColorName = simpleDto.ColorName,
                        ColorHexCode = simpleDto.ColorHexCode,
                        TransactionProductPrice = simpleDto.TransactionProductPrice,
                        TransactionQuantity = simpleDto.TransactionQuantity,
                        TransactionDate = simpleDto.TransactionDate,
                        InchargeEmployeeUsername = simpleDto.InchargeEmployeeUsername,
                        ProviderName = simpleDto.ProviderName
                    };
                    finalDtos.Add(finalDto);
                }
                return finalDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all transaction cards.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a single, fully detailed Transaction entity by its ID, including all related entities.
        /// Intended for use in a "Details" view.
        /// </summary>
        /// <param name="id">The ID of the transaction to retrieve.</param>
        /// <returns>A Transaction entity or null if not found.</returns>
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            try
            {
                // This logic correctly lives in the service layer, not the controller.
                // We use Include and ThenInclude to load all necessary related data for a detailed view.
                var transaction = await _context.Transactions
                    .Include(t => t.InchargeEmployee)
                    .Include(t => t.Provider)
                    .Include(t => t.Stock)
                    .Include(t => t.TransactionType)
                    .Include(t => t.Variation)
                        .ThenInclude(v => v.Product)
                    .Include(t => t.Variation)
                        .ThenInclude(v => v.Size)
                    .Include(t => t.Variation)
                        .ThenInclude(v => v.Color)
                    .FirstOrDefaultAsync(m => m.TransactionId == id);

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting transaction by ID: {id}.");
                throw;
            }
        }

        public async Task CreateProductImportTransactionAsync(ProductImportDTO importDto, string username)
        {
            if (importDto == null) throw new ArgumentNullException(nameof(importDto));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty.", nameof(username));

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var dbTransaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var provider = await _providerRepository.GetByIdAsync(importDto.ProviderId)
                            ?? throw new KeyNotFoundException($"Provider not found with ID: {importDto.ProviderId}.");

                        var inchargeEmployee = await _userRepository.GetByUsernameAsync(username)
                            ?? throw new KeyNotFoundException($"Incharge employee not found with username: {username}.");

                        var transactionType = await _transactionTypeRepository.FindByNameAsync(DefaultTransactionTypeName)
                            ?? throw new KeyNotFoundException($"Default transaction type '{DefaultTransactionTypeName}' not found.");

                        foreach (var variationQuantity in importDto.Variations)
                        {
                            await UpdateStockAndLogTransactionAsync(
                                variationId: variationQuantity.VariationId,
                                stockId: importDto.StockId,
                                quantityDelta: variationQuantity.Quantity,
                                transactionType: transactionType,
                                inchargeEmployee: inchargeEmployee,
                                provider: provider
                            );
                        }

                        await _context.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during product import transaction. Rolling back.");
                        await dbTransaction.RollbackAsync();
                        throw;
                    }
                }
            });
        }
    }
}
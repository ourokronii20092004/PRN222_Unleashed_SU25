using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Data;
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

        public async Task<List<TransactionCardDTO>> GetAllTransactionCardsAsync()
        {
            try
            {
                // 1. Fetch simplified DTOs
                var simpleDtos = await _transactionRepository.FindSimplifiedTransactionCardDTOsOrderByIdDescAsync();

                if (simpleDtos == null || !simpleDtos.Any())
                {
                    return new List<TransactionCardDTO>();
                }

                // 2. Collect unique Product IDs (they are Guid in SimplifiedTransactionCardDTO now)
                var productIds = simpleDtos
                    .Where(dto => dto.ProductId != Guid.Empty) // Filter out if ProductId could be Guid.Empty
                    .Select(dto => dto.ProductId)
                    .Distinct()
                    .ToList();

                // 3. Fetch Category names mapped by Product ID
                // Assuming IProductRepository has a method like this:
                // Task<Dictionary<Guid, List<string>>> GetCategoryNamesMapByProductIdsAsync(List<Guid> productIds);
                var productCategoryMap = productIds.Any()
                    ? await _productRepository.GetCategoryNamesMapByProductIdsAsync(productIds)
                    : new Dictionary<Guid, List<string>>();


                // 4. Combine data
                var finalDtos = new List<TransactionCardDTO>();
                foreach (var simpleDto in simpleDtos)
                {
                    var categoryNames = (simpleDto.ProductId != Guid.Empty && productCategoryMap.TryGetValue(simpleDto.ProductId, out var names))
                        ? names
                        : new List<string>();

                    string categoryNameString = string.Join(", ", categoryNames);
                    if (string.IsNullOrEmpty(categoryNameString))
                    {
                        categoryNameString = null; // Or "N/A"
                    }

                    // Manually construct TransactionCardDTO or use AutoMapper if a profile exists
                    // For direct construction based on Java logic:
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

        public async Task<bool> CreateStockTransactionsAsync(StockTransactionDTO stockTransactionDto)
        {
            // Use an execution strategy for resilience with explicit transaction
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                // Begin a database transaction
                // All operations within this block will be part of the same transaction.
                // If any SaveChangesAsync fails or an exception is thrown, the transaction is rolled back.
                using (var dbTransaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (stockTransactionDto.StockId == null || stockTransactionDto.ProviderId == null ||
                            string.IsNullOrWhiteSpace(stockTransactionDto.Username) ||
                            stockTransactionDto.Variations == null || !stockTransactionDto.Variations.Any())
                        {
                            _logger.LogWarning("Invalid StockTransactionDTO provided for creation.");
                            return false; // Or throw ArgumentNullException/ArgumentException
                        }

                        var provider = await _providerRepository.GetByIdAsync(stockTransactionDto.ProviderId.Value);
                        if (provider == null)
                        {
                            _logger.LogWarning($"Provider not found with ID: {stockTransactionDto.ProviderId.Value}.");
                            return false; // Or throw new KeyNotFoundException(...)
                        }

                        var stock = await _stockRepository.GetByIdAsync(stockTransactionDto.StockId.Value);
                        if (stock == null)
                        {
                            _logger.LogWarning($"Stock not found with ID: {stockTransactionDto.StockId.Value}.");
                            return false;
                        }

                        var inchargeEmployee = await _userRepository.GetByUsernameAsync(stockTransactionDto.Username);
                        if (inchargeEmployee == null)
                        {
                            _logger.LogWarning($"Incharge employee not found with username: {stockTransactionDto.Username}.");
                            return false;
                        }

                        // Assuming "IN" transaction type. In Java, it was hardcoded to ID 1.
                        // It's better to look up by a well-known name or have it passed in DTO.
                        var transactionType = await _transactionTypeRepository.FindByNameAsync(stockTransactionDto.TransactionType ?? "IN");
                        if (transactionType == null)
                        {
                            // Fallback or handle if "IN" is not found, or use a specific ID if known and static.
                            _logger.LogWarning($"TransactionType '{stockTransactionDto.TransactionType ?? "IN"}' not found.");
                            transactionType = await _transactionTypeRepository.GetByIdAsync(1); // Last resort if ID 1 is always "IN"
                            if (transactionType == null) return false;
                        }

                        // Assuming ProductStatus ID 3 is e.g. "In Stock". Better to look up by name.
                        // ProductStatus productStatusActive = await _productStatusRepository.FindByNameAsync("In Stock");
                        ProductStatus? productStatusActive = await _productStatusRepository.GetByIdAsync(3); // Java used ID 3
                        if (productStatusActive == null)
                        {
                            _logger.LogWarning($"ProductStatus for active products (ID 3 or 'In Stock') not found.");
                            // Decide how to handle: proceed without status update, or fail.
                            // For now, let's allow proceeding but log it.
                        }


                        foreach (var variationQuantity in stockTransactionDto.Variations)
                        {
                            if (variationQuantity.ProductVariationId == null || variationQuantity.Quantity == null || variationQuantity.Quantity <= 0)
                            {
                                _logger.LogWarning("Invalid ProductVariationQuantity in DTO.");
                                continue; // Skip this invalid item
                            }

                            var variation = await _variationRepository.GetByIdAsync(variationQuantity.ProductVariationId.Value);
                            if (variation == null || variation.Product == null) // Ensure product is loaded for status update
                            {
                                _logger.LogWarning($"Variation not found or product not loaded for Variation ID: {variationQuantity.ProductVariationId.Value}.");
                                // Consider if this should rollback the whole transaction or just skip this item.
                                // For now, let's assume skipping this item requires a rollback.
                                await dbTransaction.RollbackAsync();
                                return false;
                            }

                            var transaction = new Transaction
                            {
                                StockId = stock.StockId,
                                Stock = stock, // Navigation property
                                VariationId = variation.VariationId,
                                Variation = variation, // Navigation property
                                ProviderId = provider.ProviderId,
                                Provider = provider, // Navigation property
                                InchargeEmployeeId = inchargeEmployee.UserId, // Assuming UserId is the PK for User
                                InchargeEmployee = inchargeEmployee, // Navigation property
                                TransactionTypeId = transactionType.TransactionTypeId,
                                TransactionType = transactionType, // Navigation property
                                TransactionQuantity = variationQuantity.Quantity,
                                TransactionDate = DateOnly.FromDateTime(DateTime.UtcNow), // Matches Java @PrePersist
                                TransactionProductPrice = variation.VariationPrice // Matches Java @PrePersist
                            };
                            await _transactionRepository.AddAsync(transaction);
                            // No need for _transactionRepository.SaveChangesAsync() here, will be done once at the end.

                            // Update StockVariation
                            var stockVariation = await _stockVariationRepository.GetByIdAsync(variation.VariationId, stock.StockId);
                            if (stockVariation != null)
                            {
                                stockVariation.StockQuantity = (stockVariation.StockQuantity ?? 0) + variationQuantity.Quantity.Value;
                                await _stockVariationRepository.UpdateAsync(stockVariation);
                            }
                            else
                            {
                                stockVariation = new StockVariation
                                {
                                    StockId = stock.StockId,
                                    VariationId = variation.VariationId,
                                    StockQuantity = variationQuantity.Quantity.Value
                                };
                                await _stockVariationRepository.AddAsync(stockVariation);
                            }
                            // No need for _stockVariationRepository.SaveChangesAsync() here.

                            // Update Product Status
                            if (productStatusActive != null && variation.Product.ProductStatusId != productStatusActive.ProductStatusId)
                            {
                                variation.Product.ProductStatusId = productStatusActive.ProductStatusId;
                                variation.Product.ProductStatus = productStatusActive; // Set navigation property
                                // EF Core will track this change on the Product entity via the Variation
                                // You might need an explicit _productRepository.UpdateAsync(variation.Product) if Product isn't tracked.
                                // However, if `variation.Product` was included when `variation` was fetched, EF Core should track it.
                            }
                        }

                        await _context.SaveChangesAsync(); // Single save for all changes in the transaction
                        await dbTransaction.CommitAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred during bulk stock transaction creation. Rolling back.");
                        await dbTransaction.RollbackAsync();
                        // Optionally, rethrow a custom service exception or return a more detailed error object
                        return false; // Or throw;
                    }
                }
            });
        }
    }
}
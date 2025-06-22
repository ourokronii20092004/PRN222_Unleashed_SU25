using AutoMapper;
using BLL.Interfaces;
using DAL.DTO;
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
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StockService> _logger;

        public StockService(
            IStockRepository stockRepository,
            IMapper mapper,
            ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<StockDTO>> GetAllStocksAsync()
        {
            try
            {
                var stocks = await _stockRepository.GetAllAsync();
                return _mapper.Map<List<StockDTO>>(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all stocks.");
                throw;
            }
        }

        public async Task<StockDTO?> GetStockByIdAsync(int id)
        {
            try
            {
                var stock = await _stockRepository.GetByIdAsync(id);
                if (stock == null)
                {
                    return null;
                }
                return _mapper.Map<StockDTO>(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting stock by ID: {id}.");
                throw;
            }
        }

        public async Task<List<StockDetailDTO>?> GetStockDetailsAsync(int stockId)
        {
            try
            {
                // The repository method GetStockDetailsByIdAsync is already designed
                // to return List<StockDetailDTO> directly.
                var stockDetails = await _stockRepository.GetStockDetailsByIdAsync(stockId);

                // Replicating the Java service logic for filtering and null check
                if (stockDetails == null || !stockDetails.Any())
                {
                    return null;
                }

                // Filter out items with negative quantity (Java: if((Integer) row[15] < 0){ continue; })
                // This assumes Quantity is at index 15 in the Object[] in Java,
                // and now directly a property in StockDetailDTO.
                var filteredDetails = stockDetails.Where(sd => sd.Quantity.HasValue && sd.Quantity >= 0).ToList();

                if (!filteredDetails.Any())
                {
                    return null;
                }

                // Java: if (stockDetailsList.size() == 1 && stockDetailsList.get(0).getProductId() == null) { return null; }
                // This check might be more nuanced. If a valid stock exists but has no products/variations with stock,
                // it might still return a StockDetailDTO with null product fields from the LEFT JOINs.
                // The current repository query should handle cases where a stock exists but has no variations with stock,
                // potentially returning DTOs with null product/variation fields.
                // If the intent is to return null if *all* resulting DTOs have a null ProductId, the logic would be:
                if (filteredDetails.All(sd => sd.ProductId == Guid.Empty || string.IsNullOrEmpty(sd.ProductName))) // Assuming ProductId is Guid, Guid.Empty is default
                {
                    // This condition might need refinement based on exact meaning of "empty" stock detail
                    _logger.LogInformation($"Stock details for stock ID {stockId} resulted in effectively empty product data.");
                    // Depending on requirements, you might return an empty list or null.
                    // Returning empty list is often clearer than null if the stock itself exists.
                    return new List<StockDetailDTO>();
                }

                return filteredDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting stock details for ID: {stockId}.");
                throw;
            }
        }

        public async Task<StockDTO?> CreateStockAsync(StockCreateDTO stockCreateDto)
        {
            try
            {
                var stockEntity = _mapper.Map<Stock>(stockCreateDto);
                // Add any business logic, set creation timestamps etc. if not handled by DB/EF Core interceptors
                // stockEntity.CreatedAt = DateTimeOffset.UtcNow; // Example
                var createdStock = await _stockRepository.AddAsync(stockEntity);
                await _stockRepository.SaveChangesAsync(); // Or handle UoW at higher level if preferred
                return _mapper.Map<StockDTO>(createdStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock in service.");
                throw; // Or return null / custom error object
            }
        }

        public async Task<bool> UpdateStockAsync(int stockId, StockUpdateDTO stockUpdateDto)
        {
            try
            {
                var stockEntity = await _stockRepository.GetByIdAsync(stockId);
                if (stockEntity == null)
                {
                    return false; // Not found
                }

                _mapper.Map(stockUpdateDto, stockEntity); // Apply changes from DTO to entity
                                                          // stockEntity.UpdatedAt = DateTimeOffset.UtcNow; // Example
                await _stockRepository.UpdateAsync(stockEntity);
                await _stockRepository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, $"Concurrency error updating stock ID {stockId} in service.");
                throw; // Let controller handle concurrency
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating stock ID {stockId} in service.");
                throw;
            }
        }

        public async Task<bool> DeleteStockAsync(int stockId)
        {
            try
            {
                var stockEntity = await _stockRepository.GetByIdAsync(stockId);
                if (stockEntity == null)
                {
                    return false; // Not found
                }
                // Add any business logic before deletion (e.g., check for related data)
                await _stockRepository.DeleteAsync(stockId); // Assuming repo's DeleteAsync takes ID
                await _stockRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) // Catch specific exceptions like ForeignKeyConstraint if needed
            {
                _logger.LogError(ex, $"Error deleting stock ID {stockId} in service.");
                throw; // Or return false
            }
        }
    }
}
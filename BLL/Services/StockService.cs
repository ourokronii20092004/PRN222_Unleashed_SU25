using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.StockDTOs;
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
                var stockDetails = await _stockRepository.GetStockDetailsByIdAsync(stockId);

                if (stockDetails == null || !stockDetails.Any())
                {
                    return null;
                }

                var filteredDetails = stockDetails.Where(sd => sd.Quantity.HasValue && sd.Quantity >= 0).ToList();

                if (!filteredDetails.Any())
                {
                    return null;
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
                var createdStock = await _stockRepository.AddAsync(stockEntity);
                await _stockRepository.SaveChangesAsync();
                return _mapper.Map<StockDTO>(createdStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock in service.");
                throw;
            }
        }

        public async Task<bool> UpdateStockAsync(int stockId, StockUpdateDTO stockUpdateDto)
        {
            try
            {
                var stockEntity = await _stockRepository.GetByIdAsync(stockId);
                if (stockEntity == null)
                {
                    return false;
                }

                _mapper.Map(stockUpdateDto, stockEntity);
                await _stockRepository.UpdateAsync(stockEntity);
                await _stockRepository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, $"Concurrency error updating stock ID {stockId} in service.");
                throw;
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
                    return false;
                }

                await _stockRepository.DeleteAsync(stockId);
                await _stockRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting stock ID {stockId} in service.");
                throw;
            }
        }
    }
}
using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IStockService
    {
        // Corresponds to Java findAll() but returns DTOs
        Task<List<StockDTO>> GetAllStocksAsync();

        // Corresponds to Java findById() but returns a DTO
        Task<StockDTO?> GetStockByIdAsync(int id);

        // Corresponds to Java getStockDetails(Integer stockId)
        Task<List<StockDetailDTO>?> GetStockDetailsAsync(int stockId);

        Task<StockDTO?> CreateStockAsync(StockCreateDTO stockCreateDto);
        Task<bool> UpdateStockAsync(int stockId, StockUpdateDTO stockUpdateDto);
        Task<bool> DeleteStockAsync(int stockId);
    }
}
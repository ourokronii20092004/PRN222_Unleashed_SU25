using DAL.DTOs.StockDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IStockService
    {
        Task<PagedResult<StockDTO>> GetAllStocksAsync(int pageNumber, int pageSize);
        Task<StockDTO?> GetStockByIdAsync(int id);
        Task<List<StockDetailDTO>?> GetStockDetailsAsync(int stockId);
        Task<StockDTO?> CreateStockAsync(StockCreateDTO stockCreateDto);
        Task<bool> UpdateStockAsync(int stockId, StockUpdateDTO stockUpdateDto);
        Task<bool> DeleteStockAsync(int stockId);
    }
}
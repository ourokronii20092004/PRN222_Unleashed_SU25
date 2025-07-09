using DAL.DTOs.StockDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<PagedResult<Stock>> GetAllAsync(int pageNumber, int pageSize);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> AddAsync(Stock stock);
        Task UpdateAsync(Stock stock);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Corresponds to findStockDetailsById(Integer stockId)
        // The return type here is List<StockDetailDTO> directly,
        // assuming we'll project the native query results into this DTO in the implementation.
        Task<List<StockDetailDTO>> GetStockDetailsByIdAsync(int stockId);

        Task<int> SaveChangesAsync(); // For unit of work control from service if needed
    }
}
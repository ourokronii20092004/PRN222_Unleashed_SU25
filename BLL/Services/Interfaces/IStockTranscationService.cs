using DAL.DTOs.ProductDTOs;
using DAL.DTOs.StockDTOs;
using DAL.DTOs.TransactionDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IStockTransactionService
    {
        Task<List<TransactionCardDTO>> GetAllTransactionCardsAsync();
        Task<Transaction?> GetTransactionByIdAsync(int id);
        Task CreateStockTransactionsAsync(StockTransactionDTO stockTransactionDto);

        /// <summary>
        /// Creates a simplified "IN" transaction to import product quantities into a stock.
        /// </summary>
        /// <param name="importDto">The DTO containing the stock, provider, and variation quantities.</param>
        /// <param name="username">The username of the employee performing the import.</param>
        Task CreateProductImportTransactionAsync(ProductImportDTO importDto, string username);
    }
}
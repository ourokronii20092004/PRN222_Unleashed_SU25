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
        Task CreateStockTransactionsAsync(StockTransactionDTO stockTransactionDto);
        Task<Transaction?> GetTransactionByIdAsync(int id);
    }
}
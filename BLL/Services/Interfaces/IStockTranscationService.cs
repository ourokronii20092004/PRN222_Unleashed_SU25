using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IStockTransactionService
    {
        // Corresponds to Java findAllTransactionCards()
        Task<List<TransactionCardDTO>> GetAllTransactionCardsAsync();

        // Corresponds to Java createStockTransactions(StockTransactionDTO)
        // Consider a more descriptive result type than bool, e.g., a Result object with success/failure and messages
        Task<bool> CreateStockTransactionsAsync(StockTransactionDTO stockTransactionDto);
    }
}
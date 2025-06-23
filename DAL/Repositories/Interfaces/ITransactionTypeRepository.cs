using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ITransactionTypeRepository
    {
        Task<List<TransactionType>> GetAllAsync();
        Task<TransactionType?> GetByIdAsync(int id);
        Task<TransactionType?> FindByNameAsync(string name); // Potentially useful
        // Add, Update, Delete if needed, though lookup tables are often static
        // Task<TransactionType> AddAsync(TransactionType transactionType);
        // Task UpdateAsync(TransactionType transactionType);
        // Task DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
using DAL.DTOs.TransactionDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<SimplifiedTransactionCardDTO>> FindSimplifiedTransactionCardDTOsOrderByIdDescAsync();
        Task<List<Transaction>> FindByIdInOrderByIdDescAsync(List<int> ids);
        Task<bool> ExistsByProviderAsync(int providerId); // Assuming ProviderId is int

        Task<Transaction?> GetByIdAsync(int id);
        Task<Transaction> AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
        Task<List<Transaction>> GetAllAsync(); // General purpose

        Task<int> SaveChangesAsync();
    }
}
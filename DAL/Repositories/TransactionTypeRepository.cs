using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly UnleashedContext _context;

        public TransactionTypeRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionType>> GetAllAsync()
        {
            return await _context.TransactionTypes.OrderBy(tt => tt.TransactionTypeName).ToListAsync();
        }

        public async Task<TransactionType?> GetByIdAsync(int id)
        {
            return await _context.TransactionTypes.FindAsync(id);
        }

        public async Task<TransactionType?> FindByNameAsync(string name)
        {
            return await _context.TransactionTypes
                .FirstOrDefaultAsync(tt => tt.TransactionTypeName.ToLower() == name.ToLower());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
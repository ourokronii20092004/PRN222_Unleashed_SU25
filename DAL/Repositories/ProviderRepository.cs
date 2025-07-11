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
    public class ProviderRepository : IProviderRepository
    {
        private readonly UnleashedContext _context;

        public ProviderRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<Provider> AddAsync(Provider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            await _context.Providers.AddAsync(provider);
            return provider;
        }

        public Task DeleteAsync(Provider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _context.Providers.Remove(provider);
            return Task.CompletedTask; // Không cần async/await ở đây
        }

        public async Task<bool> ExistsByProviderEmailAsync(string providerEmail)
        {
            if (string.IsNullOrWhiteSpace(providerEmail))
                return false;
            string normalizedEmail = providerEmail.ToLower();
            return await _context.Providers.AnyAsync(p => p.ProviderEmail != null && p.ProviderEmail.ToLower() == normalizedEmail);
        }

        public async Task<bool> ExistsByProviderPhoneAsync(string providerPhone)
        {
            if (string.IsNullOrWhiteSpace(providerPhone))
                return false;
            return await _context.Providers.AnyAsync(p => p.ProviderPhone == providerPhone);
        }
        
        public async Task<List<Provider>> GetAllAsync()
        {
            return await _context.Providers.OrderBy(p => p.ProviderName).ToListAsync();
        }

        public async Task<Provider?> GetByIdAsync(int id)
        {
            return await _context.Providers.FirstOrDefaultAsync(p => p.ProviderId == id);
        }

        public async Task UpdateAsync(Provider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _context.Entry(provider).State = EntityState.Modified; // Or _context.Providers.Update(provider);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
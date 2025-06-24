using DAL.Data;
using DAL.DTOs.TransactionDTOs;
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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly UnleashedContext _context;

        public TransactionRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            return transaction;
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
        }

        public async Task<bool> ExistsByProviderAsync(int providerId)
        {
            return await _context.Transactions.AnyAsync(t => t.ProviderId == providerId);
        }

        public async Task<List<SimplifiedTransactionCardDTO>> FindSimplifiedTransactionCardDTOsOrderByIdDescAsync()
        {
            // Translates the JPQL query:
            // @Query(value = """
            //             SELECT DISTINCT NEW com.unleashed.dto.SimplifiedTransactionCardDTO(
            //                 t.id, p.productId, v.variationImage, p.productName, s.stockName, tt.transactionTypeName,
            //                 br.brandName, si.sizeName, col.colorName, col.colorHexCode,
            //                 t.transactionProductPrice, t.transactionQuantity, t.transactionDate,
            //                 u.userUsername, pr.providerName
            //             ) ... """)
            // This LINQ query uses navigation properties. Ensure they are correctly configured
            // and consider performance (potential for multiple joins).
            // For very complex projections, raw SQL might still be an option if LINQ becomes unwieldy or slow.

            return await _context.Transactions
                .Include(t => t.Variation)
                    .ThenInclude(v => v!.Product)
                        .ThenInclude(p => p!.Brand)
                .Include(t => t.Variation)
                    .ThenInclude(v => v!.Size)
                .Include(t => t.Variation)
                    .ThenInclude(v => v!.Color)
                .Include(t => t.Stock)
                .Include(t => t.TransactionType)
                .Include(t => t.InchargeEmployee)
                .Include(t => t.Provider)
                .OrderByDescending(t => t.TransactionId)
                .Select(t => new SimplifiedTransactionCardDTO
                {
                    TransactionId = t.TransactionId,
                    ProductId = t.Variation != null && t.Variation.Product != null ? t.Variation.Product.ProductId : System.Guid.Empty,
                    VariationImage = t.Variation != null ? t.Variation.VariationImage : null,
                    ProductName = t.Variation != null && t.Variation.Product != null ? t.Variation.Product.ProductName : null,
                    StockName = t.Stock != null ? t.Stock.StockName : null,
                    TransactionTypeName = t.TransactionType != null ? t.TransactionType.TransactionTypeName : null,
                    BrandName = (t.Variation != null && t.Variation.Product != null && t.Variation.Product.Brand != null) ? t.Variation.Product.Brand.BrandName : null,
                    SizeName = (t.Variation != null && t.Variation.Size != null) ? t.Variation.Size.SizeName : null,
                    ColorName = (t.Variation != null && t.Variation.Color != null) ? t.Variation.Color.ColorName : null,
                    ColorHexCode = (t.Variation != null && t.Variation.Color != null) ? t.Variation.Color.ColorHexCode : null,
                    TransactionProductPrice = t.TransactionProductPrice,
                    TransactionQuantity = t.TransactionQuantity,
                    TransactionDate = t.TransactionDate,
                    InchargeEmployeeUsername = t.InchargeEmployee != null ? t.InchargeEmployee.UserUsername : null,
                    ProviderName = t.Provider != null ? t.Provider.ProviderName : null
                })
                .Distinct() // Note: Distinct on a complex object might not behave as expected unless SimplifiedTransactionCardDTO implements IEquatable
                .ToListAsync();

            // If the above LINQ is too complex or Distinct() doesn't work as hoped,
            // you might need to use raw SQL for this DTO projection too, similar to StockDetailDTO,
            // by configuring SimplifiedTransactionCardDTO as a keyless entity.
        }

        public async Task<List<Transaction>> FindByIdInOrderByIdDescAsync(List<int> ids)
        {
            return await _context.Transactions
                .Where(t => ids.Contains(t.TransactionId))
                .OrderByDescending(t => t.TransactionId)
                // Include navigation properties if the caller needs them, e.g.:
                // .Include(t => t.Variation).ThenInclude(v => v.Product)
                // .Include(t => t.Stock)
                // .Include(t => t.TransactionType)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.OrderByDescending(t => t.TransactionDate).ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                // Example of including related data if often needed with a single transaction
                // .Include(t => t.Variation).ThenInclude(v => v.Product)
                // .Include(t => t.Stock)
                // .Include(t => t.TransactionType)
                // .Include(t => t.InchargeEmployee)
                // .Include(t => t.Provider)
                .FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Entry(transaction).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
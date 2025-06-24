using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly UnleashedContext _context;

        public DiscountService(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetAllDiscountsAsync()
        {
            return await _context.Discounts
                                 .Include(d => d.DiscountStatus)
                                 .Include(d => d.DiscountType)
                                 .ToListAsync();
        }

        // --- CÁC PHƯƠNG THỨC MỚI ---

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts
                                 .Include(d => d.DiscountStatus)
                                 .Include(d => d.DiscountType)
                                 .FirstOrDefaultAsync(d => d.DiscountId == id);
        }

        public async Task CreateDiscountAsync(Discount discount)
        {
            discount.DiscountCreatedAt = DateTime.UtcNow; // Set ngày tạo
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateDiscountAsync(Discount discount)
        {
            // Lấy đối tượng gốc từ database
            var existingDiscount = await _context.Discounts.FindAsync(discount.DiscountId);
            if (existingDiscount == null)
            {
                return false;
            }

            // Chỉ cập nhật các trường được gửi từ form, không chạm vào các navigation property
            _context.Entry(existingDiscount).CurrentValues.SetValues(discount);

            // Gán lại các ID từ đối tượng được bind (đảm bảo chúng được cập nhật)
            existingDiscount.DiscountStatusId = discount.DiscountStatusId;
            existingDiscount.DiscountTypeId = discount.DiscountTypeId;

            // Gán ngày cập nhật
            existingDiscount.DiscountUpdatedAt = DateTimeOffset.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteDiscountAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return false;
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
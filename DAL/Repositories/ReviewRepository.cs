using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly UnleashedContext _context;

        public ReviewRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Comments)
                .OrderBy(b => b.ReviewId)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> FindAllReviewsByProductIdAsync(Guid productId)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.ReviewId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.ReviewId == id);
        }

        public async Task<bool> HasUserReviewedProductAsync(Guid userId, Guid productId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task<List<Review>> GetReviewsWithPagingAsync(int skip, int take, string query)
        {
            var queryable = _context.Reviews    
                .Include(r => r.Product)
                .Include(r => r.User)
                .Include(r => r.Comments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(r =>
                    r.Product.ProductName.Contains(query) ||
                    r.User.UserUsername.Contains(query) ||
                    r.Order.OrderTrackingNumber.Contains(query));
            }

            return await queryable
                .OrderBy(p => p.UserId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string query)
        {
            var queryable = _context.Reviews.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(r =>
                    r.Product.ProductName.Contains(query) ||
                    r.User.UserUsername.Contains(query) ||
                    r.Order.OrderTrackingNumber.Contains(query));
            }

            return await queryable.CountAsync();
        }

        public async Task<Dictionary<Guid, double?>> GetAverageRatingsForProductsAsync(IEnumerable<Guid> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                return new Dictionary<Guid, double?>();
            }
            return await _context.Reviews
                .Where(r => productIds.Contains(r.ProductId))
                .GroupBy(r => r.ProductId)
                .Select(g => new {
                    ProductId = g.Key,
                    AverageRating = g.Average(r => (double?)r.ReviewRating)
                })
                .ToDictionaryAsync(x => x.ProductId, x => x.AverageRating);
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<Review> GetUserReviewForProductAsync(Guid userId, Guid productId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double?> GetAverageRatingByProductIdAsync(Guid productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId && r.ReviewRating.HasValue)
                .AverageAsync(r => (double?)r.ReviewRating);
        }

        public async Task<bool> HasUserOrderedProductAsync(Guid userId, Guid productId)
        {
            return await _context.Orders
                .AnyAsync(o => o.UserId == userId &&
                    _context.Reviews.Any(r => r.OrderId == o.OrderId && r.ProductId == productId));
        }

        public async Task<List<Comment>> GetCommentsByReviewIdAsync(int reviewId)
        {
            return await _context.Comments
                .Where(c => c.ReviewId == reviewId)
                .ToListAsync();
        }
    }
}
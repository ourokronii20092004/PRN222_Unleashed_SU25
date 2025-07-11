using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);
        Task DeleteAsync(Review review);
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<int> SaveChangesAsync();
        Task UpdateAsync(Review review);
        Task<List<Review>> FindAllReviewsByProductIdAsync(Guid productId);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasUserReviewedProductAsync(Guid userId, Guid productId);
        Task<List<Review>> GetReviewsWithPagingAsync(int page, int pageSize, string query);
        Task<int> GetTotalCountAsync(string query);
        Task<Dictionary<Guid, double?>> GetAverageRatingsForProductsAsync(IEnumerable<Guid> productIds);
    }
}


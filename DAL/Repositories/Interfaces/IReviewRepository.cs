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
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<Review> GetUserReviewForProductAsync(string username, Guid productId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int reviewId);
        Task DeleteAsync(Review review);
        Task<double?> GetAverageRatingByProductIdAsync(Guid productId);
        Task<bool> HasUserOrderedProductAsync(string username, Guid productId);
        Task<List<Review>> GetReviewsWithPagingAsync(int skip, int take, string query);
        Task<int> GetTotalCountAsync(string query);
        Task<Dictionary<Guid, double?>> GetAverageRatingsForProductsAsync(IEnumerable<Guid> productIds);
    }
}


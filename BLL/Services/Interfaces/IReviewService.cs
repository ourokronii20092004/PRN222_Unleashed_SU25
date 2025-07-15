using DAL.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDetailDTO>> GetReviewsByProductIdAsync(Guid productId);
        Task<ReviewDetailDTO> GetReviewByIdAsync(int reviewId);
        Task<ReviewDetailDTO> GetUserReviewForProductAsync(string username, Guid productId);
        Task CreateAsync(ReviewCreateDTO dto);
        Task UpdateAsync(int reviewId, ReviewCreateDTO dto);
        Task DeleteAsync(int reviewId, string username);
        Task DeleteAsync(int id);
        Task<DAL.Models.PagedResult<ReviewDTO>> GetReviewsWithPagingAsync(int page, int pageSize, string query);
        Task<double?> GetAverageRatingByProductIdAsync(Guid productId);
        Task<bool> HasUserOrderedProductAsync(string username, Guid productId);
        Task<IEnumerable<ReviewDetailDTO>> GetReviewsByUsernameAsync(string username);
    }
}
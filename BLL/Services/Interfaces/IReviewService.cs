using DAL.DTOs.ProductDTOs;
using DAL.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetAllAsync();
        Task<ReviewDetailDTO> GetByIdAsync(int id);
        Task<ReviewDTO> CreateAsync(ReviewCreateDTO reviewCreateDTO);
        Task UpdateAsync(int id, ReviewDTO reviewDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<ReviewDetailDTO>> GetReviewsByProductIdAsync(Guid productId);
        Task<double?> GetAverageRatingByProductIdAsync(Guid productId);
        Task<DAL.Models.PagedResult<ReviewDTO>> GetReviewsWithPagingAsync(int page, int pageSize, string query);
    }
}

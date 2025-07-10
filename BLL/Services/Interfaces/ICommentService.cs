using DAL.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task<CommentDetailDTO> GetByIdAsync(int id);
        Task<CommentDTO> CreateAsync(CommentCreateDTO commentCreateDTO);
        Task UpdateAsync(int id, CommentDTO commentDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<CommentDetailDTO>> GetCommentsByReviewIdAsync(int reviewId);
        Task<IEnumerable<CommentTreeDTO>> GetCommentTreeByReviewIdAsync(int reviewId);
    }
}

using DAL.DTOs.CommentDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDetailDTO>> GetCommentsByReviewIdAsync(int reviewId);
        Task<CommentDetailDTO> GetCommentByIdAsync(int commentId);
        Task CreateAsync(CommentCreateDTO dto);
        Task UpdateAsync(int commentId, CommentCreateDTO dto);
        Task DeleteAsync(int commentId, Guid userId);
    }
}

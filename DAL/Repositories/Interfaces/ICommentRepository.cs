using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByReviewIdAsync(int reviewId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
    }
}

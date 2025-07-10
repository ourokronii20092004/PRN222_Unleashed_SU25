using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> AddAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<int> SaveChangesAsync();
        Task UpdateAsync(Comment comment);
        Task<List<Comment>> GetCommentsByReviewIdAsync(int reviewId);
        Task<List<Comment>> GetRepliesByCommentIdAsync(int parentCommentId);
        Task<bool> ExistsAsync(int id);
    }
}

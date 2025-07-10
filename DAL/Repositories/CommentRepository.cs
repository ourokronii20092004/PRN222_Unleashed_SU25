
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
    public class CommentRepository : ICommentRepository
    {
        private readonly UnleashedContext _context;

        public CommentRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            comment.CommentCreatedAt = DateTimeOffset.UtcNow;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Comment comment)
        {
            var replies = await _context.Comments
                .Where(c => c.ParentCommentId == comment.CommentId)
                .ToListAsync();

            foreach (var reply in replies)
            {
                await DeleteAsync(reply); 
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Review)
                .Include(c => c.ParentComment)
                .OrderByDescending(c => c.CommentCreatedAt)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Review)
                .Include(c => c.ParentComment)
                .Include(c => c.InverseParentComment)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            comment.CommentUpdatedAt = DateTimeOffset.UtcNow;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsByReviewIdAsync(int reviewId)
        {
            return await _context.Comments
                .Include(c => c.Review)
                .Include(c => c.ParentComment)
                .Where(c => c.ReviewId == reviewId)
                .OrderByDescending(c => c.CommentCreatedAt)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetRepliesByCommentIdAsync(int parentCommentId)
        {
            return await _context.Comments
                .Include(c => c.Review)
                .Include(c => c.ParentComment)
                .Where(c => c.ParentCommentId == parentCommentId)
                .OrderByDescending(c => c.CommentCreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(c => c.CommentId == id);
        }
    }
}
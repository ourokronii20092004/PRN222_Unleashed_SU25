using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.CommentDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDetailDTO>> GetCommentsByReviewIdAsync(int reviewId)
        {
            var comments = await _commentRepository.GetCommentsByReviewIdAsync(reviewId);
            return _mapper.Map<IEnumerable<CommentDetailDTO>>(comments);
        }

        public async Task<CommentDetailDTO> GetCommentByIdAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            return _mapper.Map<CommentDetailDTO>(comment);
        }

        public async Task CreateAsync(CommentCreateDTO dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            await _commentRepository.AddCommentAsync(comment);
        }

        public async Task UpdateAsync(int commentId, CommentCreateDTO dto)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found.");

            // Lấy UserId từ review liên kết
            var review = comment.Review;
            if (review == null)
                throw new KeyNotFoundException("Review not found for this comment.");

            if (review.UserId != dto.UserId)
                throw new UnauthorizedAccessException("You can only edit your own comment.");

            _mapper.Map(dto, comment);
            await _commentRepository.UpdateCommentAsync(comment);
        }

        public async Task DeleteAsync(int commentId, Guid userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found.");

            var review = comment.Review;
            if (review == null)
                throw new KeyNotFoundException("Review not found for this comment.");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You can only delete your own comment.");

            await _commentRepository.DeleteCommentAsync(commentId);
        }
    }
}
using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            IReviewRepository reviewRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDetailDTO> GetByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentDetailDTO>(comment);
        }

        public async Task<CommentDTO> CreateAsync(CommentCreateDTO commentCreateDTO)
        {
            // Validate review exists if ReviewId is provided
            if (commentCreateDTO.ReviewId.HasValue &&
                !await _reviewRepository.ExistsAsync(commentCreateDTO.ReviewId.Value))
            {
                throw new KeyNotFoundException("Review not found");
            }

            // Validate parent comment exists if ParentCommentId is provided
            if (commentCreateDTO.ParentCommentId.HasValue &&
                !await _commentRepository.ExistsAsync(commentCreateDTO.ParentCommentId.Value))
            {
                throw new KeyNotFoundException("Parent comment not found");
            }

            var comment = _mapper.Map<Comment>(commentCreateDTO);
            await _commentRepository.AddAsync(comment);
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task UpdateAsync(int id, CommentDTO commentDTO)
        {
            if (id != commentDTO.CommentId)
            {
                throw new ArgumentException("ID mismatch");
            }

            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                throw new KeyNotFoundException("Comment not found");
            }

            _mapper.Map(commentDTO, existingComment);
            await _commentRepository.UpdateAsync(existingComment);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found");
            }

            await _commentRepository.DeleteAsync(comment);
        }

        public async Task<IEnumerable<CommentDetailDTO>> GetCommentsByReviewIdAsync(int reviewId)
        {
            var comments = await _commentRepository.GetCommentsByReviewIdAsync(reviewId);
            return _mapper.Map<IEnumerable<CommentDetailDTO>>(comments);
        }

        public async Task<IEnumerable<CommentTreeDTO>> GetCommentTreeByReviewIdAsync(int reviewId)
        {
            var allComments = await _commentRepository.GetCommentsByReviewIdAsync(reviewId);
            var topLevelComments = allComments.Where(c => c.ParentCommentId == null).ToList();

            var commentTree = new List<CommentTreeDTO>();
            foreach (var comment in topLevelComments)
            {
                commentTree.Add(await BuildCommentTree(comment));
            }

            return commentTree;
        }

        private async Task<CommentTreeDTO> BuildCommentTree(Comment comment)
        {
            var commentDto = _mapper.Map<CommentDTO>(comment);
            var replies = await _commentRepository.GetRepliesByCommentIdAsync(comment.CommentId);

            var commentTree = new CommentTreeDTO
            {
                Comment = commentDto,
                Replies = new List<CommentTreeDTO>()
            };

            foreach (var reply in replies)
            {
                commentTree.Replies = commentTree.Replies.Append(await BuildCommentTree(reply));
            }

            return commentTree;
        }
    }
}
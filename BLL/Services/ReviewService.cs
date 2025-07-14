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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDetailDTO>> GetReviewsByProductIdAsync(Guid productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ReviewDetailDTO>>(reviews);
        }

        public async Task<ReviewDetailDTO> GetReviewByIdAsync(int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            return _mapper.Map<ReviewDetailDTO>(review);
        }

        public async Task<ReviewDetailDTO> GetUserReviewForProductAsync(string username, Guid productId)
        {
            var review = await _reviewRepository.GetUserReviewForProductAsync(username, productId);
            return _mapper.Map<ReviewDetailDTO>(review);
        }

        public async Task CreateAsync(ReviewCreateDTO dto)
        {
            var review = _mapper.Map<Review>(dto);
            await _reviewRepository.AddReviewAsync(review);
        }

        public async Task UpdateAsync(int reviewId, ReviewCreateDTO dto)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null || review.User.UserUsername != dto.User.UserUsername)
                throw new KeyNotFoundException("Review not found or user mismatch");

            _mapper.Map(dto, review);
            await _reviewRepository.UpdateReviewAsync(review);
        }

        public async Task DeleteAsync(int reviewId, string username)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null || review.User.UserUsername != username)
                throw new KeyNotFoundException("Review not found or user mismatch");

            await _reviewRepository.DeleteReviewAsync(reviewId);
        }

        public async Task<double?> GetAverageRatingByProductIdAsync(Guid productId)
        {
            return await _reviewRepository.GetAverageRatingByProductIdAsync(productId);
        }

        public async Task<bool> HasUserOrderedProductAsync(string username, Guid productId)
        {
            return await _reviewRepository.HasUserOrderedProductAsync(username, productId);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found");
            }
            await _reviewRepository.DeleteAsync(review);
        }

        public async Task<PagedResult<ReviewDTO>> GetReviewsWithPagingAsync(int page, int pageSize, string query)
        {
            int skip = (page - 1) * pageSize;
            var reviews = await _reviewRepository.GetReviewsWithPagingAsync(skip, pageSize, query);
            var totalCount = await _reviewRepository.GetTotalCountAsync(query);
            var reviewDTOs = _mapper.Map<List<ReviewDTO>>(reviews);
            return new DAL.Models.PagedResult<ReviewDTO>
            {
                Items = reviewDTOs,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}
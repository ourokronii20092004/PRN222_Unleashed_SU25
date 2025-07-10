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

        public async Task<IEnumerable<ReviewDTO>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDetailDTO> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return _mapper.Map<ReviewDetailDTO>(review);
        }

        public async Task<ReviewDTO> CreateAsync(ReviewCreateDTO reviewCreateDTO)
        {
            var review = _mapper.Map<Review>(reviewCreateDTO);

            // Check if user has already reviewed this product
            if (await _reviewRepository.HasUserReviewedProductAsync(review.UserId, review.ProductId))
            {
                throw new InvalidOperationException("User has already reviewed this product");
            }

            await _reviewRepository.AddAsync(review);
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task UpdateAsync(int id, ReviewDTO reviewDTO)
        {
            if (id != reviewDTO.ReviewId)
            {
                throw new ArgumentException("ID mismatch");
            }

            var existingReview = await _reviewRepository.GetByIdAsync(id);
            if (existingReview == null)
            {
                throw new KeyNotFoundException("Review not found");
            }

            _mapper.Map(reviewDTO, existingReview);
            await _reviewRepository.UpdateAsync(existingReview);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found");
            }

            await _reviewRepository.DeleteAsync(review);
        }

        public async Task<IEnumerable<ReviewDetailDTO>> GetReviewsByProductIdAsync(Guid productId)
        {
            var reviews = await _reviewRepository.FindAllReviewsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ReviewDetailDTO>>(reviews);
        }

        public async Task<double?> GetAverageRatingByProductIdAsync(Guid productId)
        {
            var reviews = await _reviewRepository.FindAllReviewsByProductIdAsync(productId);
            if (reviews == null || !reviews.Any())
            {
                return null;
            }

            return reviews.Average(r => r.ReviewRating);
        }
    }
}
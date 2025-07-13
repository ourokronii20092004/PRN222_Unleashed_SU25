using BLL.Services.Interfaces;
using DAL.DTOs.CommentDTOs;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly ICommentService _commentService;
        private readonly ILogger<DetailsModel> _logger;

        public Product Product { get; set; }
        public List<ReviewDetailDTO> Reviews { get; set; } = new List<ReviewDetailDTO>();
        public double? AverageRating { get; set; }

        [BindProperty]
        public ReviewCreateDTO NewReview { get; set; } = new ReviewCreateDTO();
                            
        [BindProperty]
        public CommentCreateDTO NewComment { get; set; } = new CommentCreateDTO();

        public DetailsModel(
            IProductService productService,
            IReviewService reviewService,
            ICommentService commentService,
            ILogger<DetailsModel> logger)
        {
            _productService = productService;
            _reviewService = reviewService;
            _commentService = commentService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Product ID is required.";
                return RedirectToPage("./Index");
            }

            try
            {
                Product = await _productService.GetProductByIdAsync(id.Value);
                if (Product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID {id} not found.";
                    return NotFound();
                }

                Reviews = (List<ReviewDetailDTO>)await _reviewService.GetReviewsByProductIdAsync(id.Value);
                AverageRating = await _reviewService.GetAverageRatingByProductIdAsync(id.Value);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product details for ID {id}");
                TempData["ErrorMessage"] = "An error occurred while retrieving product details.";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAddReviewAsync(Guid productId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["ErrorMessage"] = "You must be logged in to add a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }
                var userId = Guid.Parse(userIdStr);

                var hasOrdered = await _reviewService.HasUserOrderedProductAsync(userId, productId);
                if (!hasOrdered)
                {
                    TempData["ErrorMessage"] = "You must order this product before rating.";
                    return RedirectToPage(new { id = productId });
                }

                var existingReview = await _reviewService.GetUserReviewForProductAsync(userId, productId);
                if (existingReview != null)
                {
                    TempData["ErrorMessage"] = "You have already rated this product.";
                    return RedirectToPage(new { id = productId });
                }

                NewReview.ProductId = productId;
                NewReview.UserId = userId;

                await _reviewService.CreateAsync(NewReview);
                TempData["SuccessMessage"] = "Your review has been added successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding review");
                TempData["ErrorMessage"] = "An error occurred while adding your review.";
            }

            return RedirectToPage(new { id = productId });
        }

        public async Task<IActionResult> OnPostEditReviewAsync(int reviewId, Guid productId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }
                var userId = Guid.Parse(userIdStr);

                NewReview.UserId = userId;
                await _reviewService.UpdateAsync(reviewId, NewReview);
                TempData["SuccessMessage"] = "Your review has been updated!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing review");
                TempData["ErrorMessage"] = "An error occurred while editing your review.";
            }

            return RedirectToPage(new { id = productId });
        }

        public async Task<IActionResult> OnPostDeleteReviewAsync(int reviewId, Guid productId)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["ErrorMessage"] = "You must be logged in to delete a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }
                var userId = Guid.Parse(userIdStr);

                await _reviewService.DeleteAsync(reviewId, userId);
                TempData["SuccessMessage"] = "Your review has been deleted!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                TempData["ErrorMessage"] = "An error occurred while deleting your review.";
            }

            return RedirectToPage(new { id = productId });
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int reviewId, Guid productId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["ErrorMessage"] = "You must be logged in to add a comment.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }
                var userId = Guid.Parse(userIdStr);

                NewComment.ReviewId = reviewId;
                NewComment.UserId = userId;

                await _commentService.CreateAsync(NewComment);
                TempData["SuccessMessage"] = "Your comment has been added successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                TempData["ErrorMessage"] = "An error occurred while adding your comment.";
            }

            return RedirectToPage(new { id = productId });
        }

        public async Task<IActionResult> OnPostEditCommentAsync(int commentId, int reviewId, Guid productId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit a comment.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }
                var userId = Guid.Parse(userIdStr);

                NewComment.UserId = userId;
                await _commentService.UpdateAsync(commentId, NewComment);
                TempData["SuccessMessage"] = "Your comment has been updated!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing comment");
                TempData["ErrorMessage"] = "An error occurred while editing your comment.";
            }

            return RedirectToPage(new { id = productId });
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId, int reviewId, Guid productId)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["ErrorMessage"] = "You must be logged in to delete a comment.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }
                var userId = Guid.Parse(userIdStr);

                await _commentService.DeleteAsync(commentId, userId);
                TempData["SuccessMessage"] = "Your comment has been deleted!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment");
                TempData["ErrorMessage"] = "An error occurred while deleting your comment.";
            }

            return RedirectToPage(new { id = productId });
        }
    }
}
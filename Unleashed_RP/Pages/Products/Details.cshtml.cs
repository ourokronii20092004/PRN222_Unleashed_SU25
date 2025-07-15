using BLL.Services.Interfaces;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly ILogger<DetailsModel> _logger;

        public Product Product { get; set; }
        public List<ReviewDetailDTO> Reviews { get; set; } = new List<ReviewDetailDTO>();
        public double? AverageRating { get; set; }

        [BindProperty]
        public ReviewCreateDTO NewReview { get; set; } = new ReviewCreateDTO();

        public DetailsModel(
            IProductService productService,
            IReviewService reviewService,
            ILogger<DetailsModel> logger)
        {
            _productService = productService;
            _reviewService = reviewService;
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
                string username = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "You must be logged in to add a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }

                var hasOrdered = await _reviewService.HasUserOrderedProductAsync(username, productId);
                if (!hasOrdered)
                {
                    TempData["ErrorMessage"] = "You must order this product before rating.";
                    return RedirectToPage(new { id = productId });
                }

                var existingReview = await _reviewService.GetUserReviewForProductAsync(username, productId);
                if (existingReview != null)
                {
                    TempData["ErrorMessage"] = "You have already rated this product.";
                    return RedirectToPage(new { id = productId });
                }

                if (NewReview.User == null)
                {
                    NewReview.User = new DAL.Models.User();
                }
                NewReview.ProductId = productId;
                NewReview.User.UserUsername = username;
                NewReview.ReviewCreatedAt = DateTimeOffset.UtcNow;

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
                string username = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }

                if (NewReview.User == null)
                {
                    NewReview.User = new DAL.Models.User();
                }
                NewReview.User.UserUsername = username;
                NewReview.ReviewUpdatedAt = DateTimeOffset.UtcNow;
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
                string username = HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "You must be logged in to delete a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }

                await _reviewService.DeleteAsync(reviewId, username);
                TempData["SuccessMessage"] = "Your review has been deleted!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                TempData["ErrorMessage"] = "An error occurred while deleting your review.";
            }

            return RedirectToPage(new { id = productId });
        }
    }
}
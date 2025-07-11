using BLL.Services.Interfaces;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
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
                // Lấy UserId từ thông tin đăng nhập
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to add a review.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }

                NewReview.ProductId = productId;
                NewReview.UserId = Guid.Parse(userId); 

                await _reviewService.CreateAsync(NewReview);
                TempData["SuccessMessage"] = "Your review has been added successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding review");
                TempData["ErrorMessage"] = "An error occurred while adding your review.";
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
                // Lấy UserId từ thông tin đăng nhập
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to add a comment.";
                    return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/Products/Details", new { id = productId }) });
                }

                NewComment.ReviewId = reviewId;
                NewComment.UserId = Guid.Parse(userId); 

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
    }
}
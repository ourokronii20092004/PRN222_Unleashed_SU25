using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.ReviewHistory
{
    [Filter.Filter(RequiredRoles = new[] { "CUSTOMER" })]
    public class IndexModel : PageModel
    {
        private readonly IReviewService _reviewService;

        public IndexModel(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IList<ReviewDetailDTO> Reviews { get; set; } = new List<ReviewDetailDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get username from session (same as in DetailsModel)
            string username = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "You must be logged in to view your review history.";
                return RedirectToPage("/Account/Login", new { returnUrl = Url.Page("/ReviewHistory/Index") });
            }

            try
            {
                // Get reviews by username (you might need to implement this method in your ReviewService)
                Reviews = (await _reviewService.GetReviewsByUsernameAsync(username)).ToList();

                if (Reviews == null || Reviews.Count == 0)
                {
                    TempData["InfoMessage"] = "You haven't reviewed any products yet.";
                }

                return Page();
            }
            catch (Exception ex)
            {
                // Log error and show message
                TempData["ErrorMessage"] = "An error occurred while retrieving your review history.";
                return Page();
            }
        }
    }
}
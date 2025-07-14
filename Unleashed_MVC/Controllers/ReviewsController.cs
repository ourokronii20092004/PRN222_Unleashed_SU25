using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService, ICommentService commentService, IProductService productService, IUserService userService, IOrderService orderService, IMapper mapper)
        {
            _reviewService = reviewService;
            _commentService = commentService;
            _productService = productService;
            _userService = userService;
            _orderService = orderService;
            _mapper = mapper;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(string? query, int page = 1, int pageSize = 10)
        {
            try
            {
                var pagedResult = await _reviewService.GetReviewsWithPagingAsync(page, pageSize, query);

                ViewBag.Page = pagedResult.CurrentPage;
                ViewBag.PageSize = pagedResult.PageSize;
                ViewBag.TotalCount = pagedResult.TotalCount;
                ViewBag.Query = query;

                return View(pagedResult.Items);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving reviews.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);
            if (review == null)
            {
                return NotFound();
            }
            var reviewDto = _mapper.Map<ReviewDTO>(review);

            // Get comments for this review
            var comments = await _commentService.GetCommentsByReviewIdAsync(id.Value);
            ViewBag.Comments = comments;

            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);
            if (review == null)
            {
                return NotFound();
            }
            var reviewDto = _mapper.Map<ReviewDTO>(review);
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _reviewService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting review: " + ex.Message);
                return View("Delete", await _reviewService.GetReviewByIdAsync(id));
            }
        }

    }
}
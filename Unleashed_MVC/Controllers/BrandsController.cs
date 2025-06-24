using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.BrandDTOs;

namespace Unleashed_MVC.Controllers
{
    // [Authorize] // You can apply authorization at the controller level if all actions require it
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(IBrandService brandService, IMapper mapper, ILogger<BrandsController> logger)
        {
            _brandService = brandService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Brands
        // [AllowAnonymous] // If this specific action should be public
        public async Task<IActionResult> Index()
        {
            try
            {
                // Assuming GetAllBrandsWithQuantityAsync returns List<BrandDTO>
                var brandDTOs = await _brandService.GetAllBrandsWithQuantityAsync();
                var brands = _mapper.Map<IEnumerable<Brand>>(brandDTOs); // Map to Entity for the view
                return View(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the brand index page.");
                // You could return an error view or a generic error message
                ViewData["ErrorMessage"] = "Could not load brands. Please try again later.";
                return View(new List<Brand>()); // Return empty list or error view
            }
        }

        // GET: Brands/Details/5
        [Authorize(Roles = "ADMIN,STAFF")] // Example authorization
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Brand ID not provided.");
            }

            try
            {
                var brandDto = await _brandService.GetBrandByIdAsync(id.Value);
                if (brandDto == null)
                {
                    return NotFound($"Brand with ID {id.Value} not found.");
                }
                var brand = _mapper.Map<Brand>(brandDto); // Map to Entity for the view
                return View(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching details for brand ID {id.Value}.");
                TempData["ErrorMessage"] = "Could not load brand details."; // Use TempData for redirect scenarios if needed
                return RedirectToAction(nameof(Index)); // Or return an error view
            }
        }

        // GET: Brands/Create
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            // Pass an empty DTO if your Create view is strongly typed to it for validation summary
            return View(new BrandCreateDTO());
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([Bind("BrandName,BrandDescription,BrandImageUrl,BrandWebsiteUrl")] BrandCreateDTO brandDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _brandService.CreateBrandAsync(brandDto);
                    TempData["SuccessMessage"] = "Brand created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex) // For business rule violations (e.g., duplicate name)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating a new brand.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                }
            }
            // If we got this far, something failed, redisplay form with the DTO
            return View(brandDto);
        }

        // GET: Brands/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("Brand ID not provided.");
            }

            try
            {
                var brandDto = await _brandService.GetBrandByIdAsync(id.Value);
                if (brandDto == null)
                {
                    return NotFound($"Brand with ID {id.Value} not found for editing.");
                }
                // Map the BrandDTO (from service) to a BrandUpdateDTO for the Edit view
                var brandUpdateDto = _mapper.Map<BrandUpdateDTO>(brandDto);
                return View(brandUpdateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching brand ID {id.Value} for edit.");
                TempData["ErrorMessage"] = "Could not load brand for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int id, [Bind("BrandName,BrandDescription,BrandImageUrl,BrandWebsiteUrl")] BrandUpdateDTO brandDto)
        {
            // It's good practice to ensure the ID from the route matches any ID potentially in the DTO,
            // or better yet, only rely on the route ID for identifying the entity.

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedBrand = await _brandService.UpdateBrandAsync(id, brandDto);
                    if (updatedBrand == null)
                    {
                        // This case can happen if the brand was deleted between GET and POST
                        return NotFound($"Brand with ID {id} not found during update attempt.");
                    }
                    TempData["SuccessMessage"] = "Brand updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogWarning(ex, $"Concurrency issue updating brand ID {id}.");
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                        + "was modified by another user. The edit operation was canceled. Please try again.");
                    // Optionally, you could re-fetch the current data and send it back to the view
                    // var currentBrandDto = await _brandService.GetBrandByIdAsync(id);
                    // var currentUpdateDto = _mapper.Map<BrandUpdateDTO>(currentBrandDto);
                    // return View(currentUpdateDto); // Or return View(brandDto) with the user's attempted values
                }
                catch (InvalidOperationException ex) // For business rule violations (e.g., duplicate name on another brand)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating brand ID {id}.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating. Please try again.");
                }
            }
            // If we got this far, something failed (model state invalid or exception), redisplay form with the DTO
            return View(brandDto);
        }

        // GET: Brands/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id, bool error = false)
        {
            if (id == null)
            {
                return NotFound("Brand ID not provided.");
            }

            if (error && TempData.ContainsKey("ErrorMessage")) // Check if redirected with an error
            {
                ViewData["ErrorMessageFromDeletePost"] = TempData["ErrorMessage"];
            }

            try
            {
                var brandDto = await _brandService.GetBrandByIdAsync(id.Value);
                if (brandDto == null)
                {
                    return NotFound($"Brand with ID {id.Value} not found for deletion.");
                }
                var brand = _mapper.Map<Brand>(brandDto); // Map to Entity for the Delete confirmation view
                return View(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching brand ID {id.Value} for delete confirmation.");
                TempData["ErrorMessage"] = "Could not load brand for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = await _brandService.DeleteBrandAsync(id);
                if (!deleted)
                {
                    // This implies the brand was not found by the service during the delete attempt
                    TempData["ErrorMessage"] = $"Brand with ID {id} could not be found to delete.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["SuccessMessage"] = "Brand deleted successfully.";
            }
            catch (InvalidOperationException ex) // e.g., "Cannot delete brand because it has linked products."
            {
                _logger.LogWarning($"Business rule violation while deleting brand ID {id}: {ex.Message}");
                // To show this on the Delete confirmation page, redirect back to the GET Delete action
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id = id, error = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting brand ID {id}.");
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the brand.";
                // Optionally redirect to Delete GET action, or Index
                return RedirectToAction(nameof(Delete), new { id = id, error = true });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
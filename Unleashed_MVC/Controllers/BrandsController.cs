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
using BLL.Utilities.Interfaces;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter]
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandsController> _logger;
        private readonly IImageUploader _imageUploader;

        public BrandsController(IBrandService brandService, IMapper mapper, ILogger<BrandsController> logger, IImageUploader imageUploader)
        {
            _brandService = brandService;
            _mapper = mapper;
            _logger = logger;
            _imageUploader = imageUploader;
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
        //[Authorize(Roles = "ADMIN,STAFF")] // Example authorization
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
        //[Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            // Pass an empty DTO if your Create view is strongly typed to it for validation summary
            return View(new BrandCreateDTO());
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(BrandCreateDTO brandDto)
        {
            if (ModelState.IsValid)
            {
                if (brandDto.BrandImageFile != null && brandDto.BrandImageFile.Length > 0)
                {
                    var uploadResult = await _imageUploader.UploadImageAsync(brandDto.BrandImageFile);
                    if (uploadResult != null)
                    {
                        brandDto.BrandImageUrl = uploadResult.Url;
                    }
                    else
                    {
                        ModelState.AddModelError("BrandImageFile", "Image upload failed. Please try again.");
                        return View(brandDto);
                    }
                }

                try
                {
                    await _brandService.CreateBrandAsync(brandDto);
                    TempData["SuccessMessage"] = "Brand created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating a new brand.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                }
            }
            return View(brandDto);
        }

        // GET: Brands/Edit/5
        //[Authorize(Roles = "ADMIN")]
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

                var brandUpdateDto = _mapper.Map<BrandUpdateDTO>(brandDto);

                ViewData["BrandId"] = brandDto.BrandId;
                ViewData["CreatedAtDisplay"] = brandDto.BrandCreatedAt.HasValue ? brandDto.BrandCreatedAt.Value.ToString("F") : "N/A";
                ViewData["UpdatedAtDisplay"] = brandDto.BrandUpdatedAt.HasValue ? brandDto.BrandUpdatedAt.Value.ToString("F") : "N/A";

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
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int id, BrandUpdateDTO brandDto)
        {
            if (ModelState.IsValid)
            {
                if (brandDto.BrandImageFile != null && brandDto.BrandImageFile.Length > 0)
                {
                    var uploadResult = await _imageUploader.UploadImageAsync(brandDto.BrandImageFile);
                    if (uploadResult != null)
                    {
                        brandDto.BrandImageUrl = uploadResult.Url;
                    }
                    else
                    {
                        ModelState.AddModelError("BrandImageFile", "New image upload failed. Please try again.");
                        return View(brandDto);
                    }
                }
                try
                {
                    await _brandService.UpdateBrandAsync(id, brandDto);
                    TempData["SuccessMessage"] = "Brand updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating brand ID {id}.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating.");
                }
            }
            return View(brandDto);
        }

        // GET: Brands/Delete/5
        //[Authorize(Roles = "ADMIN")]
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
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = await _brandService.DeleteBrandAsync(id);
                if (!deleted)
                {
                    TempData["ErrorMessage"] = $"Brand with ID {id} could not be found to delete.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["SuccessMessage"] = "Brand deleted successfully.";
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Business rule violation while deleting brand ID {id}: {ex.Message}");
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id = id, error = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting brand ID {id}.");
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the brand.";
                return RedirectToAction(nameof(Delete), new { id = id, error = true });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
using AutoMapper;
using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Utilities.Interfaces;
using DAL.Data;
using DAL.DTOs.BrandDTOs;
using DAL.DTOs.CategoryDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IImageUploader _imageUploader;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, ILogger<CategoriesController> logger, IImageUploader imageUploader)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
            _imageUploader = imageUploader;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categoryDTOs = await _categoryService.GetAllCategoriesAsync();
            var categories = _mapper.Map<IEnumerable<Category>>(categoryDTOs);
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var categoryDTO = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryDTO == null)
            {
                return NotFound();
            }
            var category = _mapper.Map<Category>(categoryDTO);
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View(new CategoryCreateDTO());
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                if (categoryDTO.CategoryImageFile != null && categoryDTO.CategoryImageFile.Length > 0)
                {
                    var uploadResult = await _imageUploader.UploadImageAsync(categoryDTO.CategoryImageFile);
                    if (uploadResult != null)
                    {
                        categoryDTO.CategoryImageUrl = uploadResult.Url;
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryImageFile", "Image upload failed. Please try again.");
                        return View(categoryDTO);
                    }
                }
                await _categoryService.CreateCategoryAsync(categoryDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(categoryDTO);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("Category ID not provided.");
            }
            var categoryDTO = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (categoryDTO == null)
            {
                return NotFound();
            }
            var categoryUpdateDTO = _mapper.Map<CategoryUpdateDTO>(categoryDTO);
            ViewData["CategoryId"] = categoryDTO.CategoryId;
            ViewData["CreatedAtDisplay"] = categoryDTO.CategoryCreatedAt.HasValue ? categoryDTO.CategoryCreatedAt.Value.ToString("F") : "N/A";
            ViewData["UpdatedAtDisplay"] = categoryDTO.CategoryUpdatedAt.HasValue ? categoryDTO.CategoryUpdatedAt.Value.ToString("F") : "N/A";
            return View(categoryUpdateDTO);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryUpdateDTO categoryDTO)
        {

            if (ModelState.IsValid)
            {
                if (categoryDTO.CategoryImageFile != null && categoryDTO.CategoryImageFile.Length > 0)
                {
                    var uploadResult = await _imageUploader.UploadImageAsync(categoryDTO.CategoryImageFile);
                    if (uploadResult != null)
                    {
                        categoryDTO.CategoryImageUrl = uploadResult.Url;
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryImageFile", "New image upload failed. Please try again.");
                        return View(categoryDTO);
                    }
                }
                await _categoryService.UpdateCategoryAsync(id, categoryDTO);
                TempData["SuccessMessage"] = "Brand updated successfully!";
                return RedirectToAction(nameof(Index));

            }
            return View(categoryDTO);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id, bool error = false)
        {
            var categoryDTO = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryDTO == null)
            {
                return NotFound();
            }
            var category = _mapper.Map<Category>(categoryDTO);
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using BLL.Services.Interfaces;
using BLL.Utilities.Interfaces;
using DAL.DTOs.ProductDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IImageUploader _imageUploader;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductService productService,
            IImageUploader imageUploader)
        {
            _productService = productService;
            _imageUploader = imageUploader;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? query, int page = 1, int pageSize = 12)
        {
            try
            {
                var pagedResult = await _productService.GetProductsWithPagingAsync(page, pageSize, query);

                ViewBag.Page = pagedResult.CurrentPage;
                ViewBag.PageSize = pagedResult.PageSize;
                ViewBag.TotalCount = pagedResult.TotalCount;
                ViewBag.Query = query;

                return View(pagedResult.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                TempData["ErrorMessage"] = "An error occurred while retrieving products.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Product ID is required.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id.Value);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID {id} not found.";
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product details for ID {id}");
                TempData["ErrorMessage"] = "An error occurred while retrieving product details.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var dropdowns = await _productService.GetProductDropdownsAsync();
                ViewBag.BrandId = new SelectList(dropdowns.Brands, "BrandId", "BrandName");
                ViewBag.ProductStatusId = new SelectList(dropdowns.Statuses, "ProductStatusId", "ProductStatusName");
                ViewBag.SizeId = new SelectList(dropdowns.Sizes, "SizeId", "SizeName");
                ViewBag.ColorId = new SelectList(dropdowns.Colors, "ColorId", "ColorName");
                ViewBag.Categories = dropdowns.Categories;
                var model = new ProductDTO
                {
                    Variations = new List<ProductDTO.ProductVariationDTO> { new() }
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create form");
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await ReloadDropdowns(productDTO);
                    return View(productDTO);
                }

                // Check if ProductCode already exists
                var existingProduct = await _productService.GetProductByCodeAsync(productDTO.ProductCode);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("ProductCode", "Product with this code already exists.");
                    await ReloadDropdowns(productDTO);
                    return View(productDTO);
                }

                foreach (var variation in productDTO.Variations)
                {
                    if (variation.ProductVariationFile != null && variation.ProductVariationFile.Length > 0)
                    {
                        var uploadResult = await _imageUploader.UploadImageAsync(variation.ProductVariationFile);
                        if (uploadResult == null)
                        {
                            ModelState.AddModelError("", "Failed to upload variation image.");
                            await ReloadDropdowns(productDTO);
                            return View(productDTO);
                        }
                        variation.ProductVariationImageUrl = uploadResult.Url;
                    }
                }

                var product = await _productService.CreateProductAsync(productDTO);
                TempData["SuccessMessage"] = "Product created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError("", "An error occurred while creating the product.");
                await ReloadDropdowns(productDTO);
                return View(productDTO);
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id.Value);
                if (product == null)
                {
                    return NotFound();
                }

                var dropdowns = await _productService.GetProductDropdownsAsync();
                ViewBag.BrandId = new SelectList(dropdowns.Brands, "BrandId", "BrandName", product.BrandId);
                ViewBag.ProductStatusId = new SelectList(dropdowns.Statuses, "ProductStatusId", "ProductStatusName", product.ProductStatusId);
                ViewBag.SizeId = new SelectList(dropdowns.Sizes, "SizeId", "SizeName");
                ViewBag.ColorId = new SelectList(dropdowns.Colors, "ColorId", "ColorName");
                ViewBag.Categories = dropdowns.Categories;

                var productDTO = MapProductToDTO(product);
                return View(productDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading edit form for product ID {id}");
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
            {
                return NotFound();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    await ReloadDropdowns(productDTO);
                    return View(productDTO);
                }

                // Handle image uploads
                foreach (var variation in productDTO.Variations)
                {
                    if (variation.ProductVariationFile != null && variation.ProductVariationFile.Length > 0)
                    {
                        var uploadResult = await _imageUploader.UploadImageAsync(variation.ProductVariationFile);
                        if (uploadResult == null)
                        {
                            ModelState.AddModelError("", "Failed to upload variation image.");
                            await ReloadDropdowns(productDTO);
                            return View(productDTO);
                        }
                        variation.ProductVariationImageUrl = uploadResult.Url;
                    }
                }

                var updatedProduct = await _productService.UpdateProductAsync(id, productDTO);
                if (updatedProduct == null)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Product updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product ID {id}");
                ModelState.AddModelError("", "An error occurred while updating the product.");
                await ReloadDropdowns(productDTO);
                return View(productDTO);
            }
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id.Value);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading delete confirmation for product ID {id}");
                TempData["ErrorMessage"] = "An error occurred while loading the delete confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid ProductId, string deleteType)
        {
            try
            {
                if (deleteType == "soft")
                {
                    await _productService.SoftDeleteProductAsync(ProductId);
                    TempData["SuccessMessage"] = "Product has been deactivated successfully.";
                }
                else if (deleteType == "hard")
                {
                    var deleted = await _productService.DeleteProductAsync(ProductId);
                    if (!deleted)
                    {
                        return NotFound();
                    }
                    TempData["SuccessMessage"] = "Product has been permanently deleted successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product ID {ProductId}");
                TempData["ErrorMessage"] = "An error occurred while deleting the product.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task ReloadDropdowns(ProductDTO productDTO)
        {
            var dropdowns = await _productService.GetProductDropdownsAsync();

            ViewBag.BrandId = new SelectList(dropdowns.Brands, "BrandId", "BrandName", productDTO.BrandId);
            ViewBag.ProductStatusId = new SelectList(dropdowns.Statuses, "ProductStatusId", "ProductStatusName", productDTO.ProductStatusId);
            ViewBag.SizeId = new SelectList(dropdowns.Sizes, "SizeId", "SizeName");
            ViewBag.ColorId = new SelectList(dropdowns.Colors, "ColorId", "ColorName");
            ViewBag.Categories = dropdowns.Categories;
            productDTO.Variations ??= new List<ProductDTO.ProductVariationDTO> { new() };
        }

        private ProductDTO MapProductToDTO(Product product)
        {
            return new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                ProductDescription = product.ProductDescription,
                BrandId = product.BrandId,
                ProductStatusId = product.ProductStatusId,
                CreatedAt = product.ProductCreatedAt,
                UpdatedAt = product.ProductUpdatedAt,
                SelectedCategoryIds = product.Categories?.Select(c => c.CategoryId).ToList() ?? new List<int>(),
                Variations = product.Variations?.Select(v => new ProductDTO.ProductVariationDTO
                {
                    SizeId = v.SizeId,
                    ColorId = v.ColorId,
                    ProductPrice = v.VariationPrice,
                    ProductVariationImageUrl = v.VariationImage
                }).ToList() ?? new List<ProductDTO.ProductVariationDTO>(),
                Categories = product.Categories?.ToList() ?? new List<Category>()
            };
        }
    }
}
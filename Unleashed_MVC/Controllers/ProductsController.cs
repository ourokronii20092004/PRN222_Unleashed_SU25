using BLL.Services.Interfaces;
using DAL.DTOs.ProductDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandRepository _brandRepository;     
        private readonly IProductStatusRepository _productStatusRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;

        public ProductsController(IProductService productService, IBrandRepository brandRepository, IProductStatusRepository productStatusRepository, IVariationRepository variationRepository, IColorRepository colorRepository, ISizeRepository sizeRepository)
        {
            _productService = productService;
            _brandRepository = brandRepository;
            _productStatusRepository = productStatusRepository;
            _variationRepository = variationRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;

            var products = await _productService.GetAllProductsAsync(); 
            var pagedProducts = products.Skip(skip).Take(pageSize).ToList(); 

            var totalCount = products.Count();

            ViewBag.TotalCount = totalCount;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(pagedProducts); 
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "BrandId", "BrandName");
            ViewBag.ProductStatusId = new SelectList(await _productStatusRepository.GetAllAsync(), "ProductStatusId", "ProductStatusName");
            ViewBag.SizeId = new SelectList(await _sizeRepository.GetAllAsync(), "SizeId", "SizeName");
            ViewBag.ColorId = new SelectList(await _colorRepository.GetAllAsync(), "ColorId", "ColorName");

            var model = new ProductDTO
            {
                Variations = new List<ProductDTO.ProductVariationDTO>
                
        {
            new ProductDTO.ProductVariationDTO()
        }
            };
            return View(model);
        }


        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            // Check for duplicate ProductCode before model validation
            var existingProduct = await _productService.GetProductByCodeAsync(productDTO.ProductCode);
            if (existingProduct != null)
            {
                ModelState.AddModelError("ProductCode", "Product code already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productDTO.ProductId = Guid.NewGuid();
                    await _productService.CreateProductAsync(productDTO);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating product: " + ex.Message);
                }
            }

            // Reload dropdowns if there's an error
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "BrandId", "BrandName", productDTO.BrandId);
            ViewBag.ProductStatusId = new SelectList(await _productStatusRepository.GetAllAsync(), "ProductStatusId", "StatusName", productDTO.ProductStatusId);
            ViewBag.SizeId = new SelectList(await _sizeRepository.GetAllAsync(), "SizeId", "SizeName");
            ViewBag.ColorId = new SelectList(await _colorRepository.GetAllAsync(), "ColorId", "ColorName");

            // Ensure Variations isn't null
            productDTO.Variations ??= new List<ProductDTO.ProductVariationDTO> { new ProductDTO.ProductVariationDTO() };

            return View(productDTO);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
                var product = await _productService.GetProductByIdAsync(id.Value);
                if (product == null)
                {
                    return NotFound();
                }

                // Load dropdown data with null checks
                var brands = await _brandRepository.GetAllAsync() ?? new List<Brand>();
                var statuses = await _productStatusRepository.GetAllAsync() ?? new List<ProductStatus>();
                var sizes = await _sizeRepository.GetAllAsync() ?? new List<Size>();
                var colors = await _colorRepository.GetAllAsync() ?? new List<Color>();

                ViewBag.BrandId = new SelectList(brands, "BrandId", "BrandName", product.BrandId);
                ViewBag.ProductStatusId = new SelectList(statuses, "ProductStatusId", "ProductStatusName", product.ProductStatusId);
                ViewBag.SizeId = new SelectList(sizes, "SizeId", "SizeName");
                ViewBag.ColorId = new SelectList(colors, "ColorId", "ColorName");

                var productDTO = new ProductDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductCode = product.ProductCode,
                    ProductDescription = product.ProductDescription,
                    BrandId = product.BrandId,
                    ProductStatusId = product.ProductStatusId,
                    CreatedAt = product.ProductCreatedAt,
                    UpdatedAt = product.ProductUpdatedAt,
                    Variations = product.Variations?.Select(v => new ProductDTO.ProductVariationDTO
                    {
                        SizeId = v.SizeId,
                        ColorId = v.ColorId,
                        ProductPrice = v.VariationPrice,
                        ProductVariationImage = v.VariationImage
                    }).ToList() ?? new List<ProductDTO.ProductVariationDTO>()
                };

                // Ensure at least one variation exists
                if (!productDTO.Variations.Any())
                {
                    productDTO.Variations.Add(new ProductDTO.ProductVariationDTO());
                }

                return View(productDTO);
            
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
            {
                return NotFound(); // Kiểm tra nếu id không khớp
            }

            var brands = await _brandRepository.GetAllAsync() ?? new List<Brand>();
            var statuses = await _productStatusRepository.GetAllAsync() ?? new List<ProductStatus>();
            var sizes = await _sizeRepository.GetAllAsync() ?? new List<Size>();
            var colors = await _colorRepository.GetAllAsync() ?? new List<Color>();

            try
            {
                if (ModelState.IsValid)
                {
                    var existingProduct = await _productService.GetProductByCodeAsync(productDTO.ProductCode);
                    if (existingProduct != null && existingProduct.ProductId != id)
                    {
                        ModelState.AddModelError("ProductCode", "Product code already exists");
                    }
                    else
                    {
                        productDTO.UpdatedAt = DateTimeOffset.UtcNow;
                        productDTO.CreatedAt ??= DateTimeOffset.UtcNow;

                        await _productService.UpdateProductAsync(id, productDTO);
                        return RedirectToAction(nameof(Index)); // Redirect to Index after success
                    }
                }

                // If validation fails, reload dropdowns
                ViewBag.BrandId = new SelectList(brands, "BrandId", "BrandName", productDTO.BrandId);
                ViewBag.ProductStatusId = new SelectList(statuses, "ProductStatusId", "ProductStatusName", productDTO.ProductStatusId);
                ViewBag.SizeId = new SelectList(sizes, "SizeId", "SizeName");
                ViewBag.ColorId = new SelectList(colors, "ColorId", "ColorName");

                // Ensure variations exist
                productDTO.Variations ??= new List<ProductDTO.ProductVariationDTO>();
                if (!productDTO.Variations.Any())
                {
                    productDTO.Variations.Add(new ProductDTO.ProductVariationDTO());
                }

                return View("Edit", productDTO); // Explicitly specify view name
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the product. Please try again.");

                // Reload ViewBag on error
                ViewBag.BrandId = new SelectList(brands, "BrandId", "BrandName", productDTO.BrandId);
                ViewBag.ProductStatusId = new SelectList(statuses, "ProductStatusId", "ProductStatusName", productDTO.ProductStatusId);
                ViewBag.SizeId = new SelectList(sizes, "SizeId", "SizeName");
                ViewBag.ColorId = new SelectList(colors, "ColorId", "ColorName");

                return View("Edit", productDTO); // Explicitly specify view name
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (deleted)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}

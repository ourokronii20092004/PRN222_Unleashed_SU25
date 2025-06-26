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
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandRepository _brandRepository;     
        private readonly IProductStatusRepository _productStatusRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;

        // Tiêm IProductService vào trong constructor
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
            if (ModelState.IsValid)
            {
                productDTO.ProductId = Guid.NewGuid();
                await _productService.CreateProductAsync(productDTO);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "BrandId", "BrandName", productDTO.BrandId);
            ViewBag.ProductStatusId = new SelectList(await _productStatusRepository.GetAllAsync(), "ProductStatusId", "ProductStatusName", productDTO.ProductStatusId);
            ViewBag.SizeId = new SelectList(await _sizeRepository.GetAllAsync(), "SizeId", "SizeName");
            ViewBag.ColorId = new SelectList(await _colorRepository.GetAllAsync(), "ColorId", "ColorName");

            return View(productDTO);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,BrandId,ProductStatusId,ProductName,ProductCode,ProductDescription,ProductCreatedAt,ProductUpdatedAt")] ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(id, productDTO);
                return RedirectToAction(nameof(Index));
            }

            return View(productDTO);
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

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

        // Tiêm IProductService vào trong constructor
        public ProductsController(IProductService productService, IBrandRepository brandRepository, IProductStatusRepository productStatusRepository)
        {
            _productService = productService;
            _brandRepository = brandRepository;
            _productStatusRepository = productStatusRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
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
            // Lấy danh sách các Brand và ProductStatus
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "BrandId", "BrandName");
            ViewBag.ProductStatusId = new SelectList(await _productStatusRepository.GetAllAsync(), "ProductStatusId", "ProductStatusName");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,BrandId,ProductStatusId,ProductName,ProductCode,ProductDescription,ProductCreatedAt,ProductUpdatedAt")] ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                productDTO.ProductId = Guid.NewGuid();  // Tạo mới ProductId
                await _productService.CreateProductAsync(productDTO);  // Gọi service để tạo sản phẩm
                return RedirectToAction(nameof(Index));  // Sau khi tạo, chuyển hướng về danh sách sản phẩm
            }

            // Nếu có lỗi, trả về view và giữ lại dữ liệu đã nhập
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "BrandId", "BrandName", productDTO.BrandId);
            ViewBag.ProductStatusId = new SelectList(await _productStatusRepository.GetAllAsync(), "ProductStatusId", "ProductStatusName", productDTO.ProductStatusId);

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

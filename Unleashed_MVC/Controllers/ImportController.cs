using BLL.Services.Interfaces;
using DAL.DTOs.ProductDTOs;
using DAL.DTOs.StockDTOs;
using DAL.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unleashed_MVC.Filter;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter]
    public class ImportController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IStockTransactionService _stockTransactionService;
        private readonly IProviderService _providerService;
        private readonly IProductService _productService;
        private readonly IVariationService _variationService;
        private readonly ILogger<ImportController> _logger;

        public ImportController(
            IStockService stockService,
            IStockTransactionService stockTransactionService,
            IProviderService providerService,
            IProductService productService,
            IVariationService variationService,
            ILogger<ImportController> logger)
        {
            _stockService = stockService;
            _stockTransactionService = stockTransactionService;
            _providerService = providerService;
            _productService = productService;
            _variationService = variationService;
            _logger = logger;
        }

        [HttpGet("Import/SelectProducts/{id}")]
        public async Task<IActionResult> SelectProducts(int? id)
        {
            if (id == null) return NotFound("Stock ID not provided.");
            var stock = await _stockService.GetStockByIdAsync(id.Value);
            if (stock == null) return NotFound($"Stock with ID {id.Value} not found.");
            var productsForSelection = await _productService.GetProductsForImportSelectionAsync(id.Value);
            var viewModel = new SelectProductsViewModel
            {
                StockId = stock.StockId,
                StockName = stock.StockName,
                Products = productsForSelection.Select(p => new ProductSelectionItem
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    BrandName = p.BrandName,
                    FirstVariationImageUrl = p.FirstVariationImageUrl,
                    CategoryNames = p.CategoryNames,
                    FirstVariationPrice = p.FirstVariationPrice,
                    QuantityInStock = p.QuantityInStock,
                    IsSelected = false
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost("Import/SelectProducts/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProducts(SelectProductsViewModel viewModel)
        {
            var selectedProductIds = viewModel.Products
                .Where(p => p.IsSelected)
                .Select(p => p.ProductId)
                .ToList();
            if (!selectedProductIds.Any())
            {
                ModelState.AddModelError("", "You must select at least one product to continue.");
                return View(viewModel);
            }
            return RedirectToAction("EnterQuantities", new { stockId = viewModel.StockId, productIds = selectedProductIds });
        }

        [HttpGet("Import/EnterQuantities")]
        public async Task<IActionResult> EnterQuantities(int stockId, [FromQuery] Guid[] productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                return BadRequest("No products were selected.");
            }
            var stock = await _stockService.GetStockByIdAsync(stockId);
            if (stock == null) return NotFound("Stock not found.");
            var variations = await _variationService.GetVariationDetailsForProductsAsync(productIds.ToList());
            var providers = await _providerService.GetAllProvidersAsync();
            var groupedProducts = variations
                .GroupBy(v => v.ProductName)
                .Select(group => new ProductImportGroup
                {
                    ProductName = group.Key,
                    BrandName = group.First().BrandName,
                    VariationsToImport = group.Select(v => new ProductVariationDetail
                    {
                        VariationId = v.VariationId,
                        SizeName = v.SizeName,
                        ColorName = v.ColorName,
                        Price = v.VariationPrice,
                        ImportQuantity = 0,
                        VariationImageUrl = v.VariationImageUrl,
                        ColorHexCode = v.ColorHexCode
                    }).ToList()
                }).ToList();
            var viewModel = new ProductImportViewModel
            {
                StockId = stockId,
                StockName = stock.StockName,
                Providers = providers.Select(p => new SelectListItem
                {
                    Value = p.ProviderId.ToString(),
                    Text = p.ProviderName
                }).ToList(),
                ProductGroups = groupedProducts
            };
            return View("EnterQuantities", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterQuantities(ProductImportViewModel viewModel)
        {
            var itemsToImport = viewModel.ProductGroups
                                         .SelectMany(group => group.VariationsToImport)
                                         .Where(v => v.ImportQuantity > 0)
                                         .ToList();

            if (!itemsToImport.Any())
            {
                ModelState.AddModelError("", "Please enter a quantity for at least one product variation.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var username = HttpContext.Session.GetString("username");
                    if (string.IsNullOrEmpty(username))
                    {
                        throw new InvalidOperationException("Username not found in session. Please log in again.");
                    }

                    var importDto = new ProductImportDTO
                    {
                        StockId = viewModel.StockId,
                        ProviderId = viewModel.ProviderId,
                        Variations = itemsToImport.Select(v => new VariationQuantityDTO
                        {
                            VariationId = v.VariationId,
                            Quantity = v.ImportQuantity
                        }).ToList()
                    };

                    await _stockTransactionService.CreateProductImportTransactionAsync(importDto, username);
                    TempData["SuccessMessage"] = "Products imported successfully!";
                    return RedirectToAction("Details", "Stocks", new { id = viewModel.StockId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during product import.");
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                }
            }

            var providers = await _providerService.GetAllProvidersAsync();
            viewModel.Providers = providers.Select(p => new SelectListItem
            {
                Value = p.ProviderId.ToString(),
                Text = p.ProviderName
            }).ToList();

            // This part is tricky because we need to rebuild the grouped structure if we fail.
            // For now, we return the viewModel as-is, but a full implementation would
            // re-fetch and re-group the variations.
            return View("EnterQuantities", viewModel);
        }
    }
}
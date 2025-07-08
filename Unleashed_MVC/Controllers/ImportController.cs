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

namespace Unleashed_MVC.Controllers
{
    public class ImportController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IStockTransactionService _stockTransactionService;
        private readonly IProviderService _providerService;
        private readonly ILogger<ImportController> _logger;

        public ImportController(
            IStockService stockService,
            IStockTransactionService stockTransactionService,
            IProviderService providerService,
            ILogger<ImportController> logger)
        {
            _stockService = stockService;
            _stockTransactionService = stockTransactionService;
            _providerService = providerService;
            _logger = logger;
        }

        [HttpGet("Import/Create/{id}")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound("Stock ID not provided.");
            }

            var stockDetails = await _stockService.GetStockDetailsAsync(id.Value);
            if (stockDetails == null || !stockDetails.Any())
            {
                return NotFound($"Stock with ID {id.Value} not found or has no items.");
            }

            var providers = await _providerService.GetAllProvidersAsync();

            var viewModel = new ProductImportViewModel
            {
                StockId = id.Value,
                StockName = stockDetails.First().StockName,
                Providers = providers.Select(p => new SelectListItem
                {
                    Value = p.ProviderId.ToString(),
                    Text = p.ProviderName
                }).ToList(),
                VariationsToImport = stockDetails
                    .Where(detail => detail.VariationId.HasValue)
                    .Select(detail => new ProductVariationDetail
                    {
                        VariationId = detail.VariationId.Value,
                        ProductName = detail.ProductName,
                        BrandName = detail.BrandName,
                        SizeName = detail.SizeName,
                        ColorName = detail.ColorName,
                        CurrentQuantity = detail.Quantity ?? 0,
                        ImportQuantity = 0
                    }).ToList()
            };

            if (!viewModel.VariationsToImport.Any())
            {
                return NotFound($"The stock '{viewModel.StockName}' exists, but has no products associated with it to import.");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImportViewModel viewModel)
        {
            var itemsToImport = viewModel.VariationsToImport
                                         .Where(v => v.ImportQuantity > 0)
                                         .ToList();

            if (!itemsToImport.Any())
            {
                ModelState.AddModelError("", "Please enter a quantity for at least one product to import.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var username = User.Identity?.Name;
                    if (string.IsNullOrEmpty(username))
                    {
                        throw new InvalidOperationException("User is not authenticated.");
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

            return View(viewModel);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using BLL.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DAL.DTOs.StockDTOs;
using DAL.Models.ViewModels;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter]
    public class StocksController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IStockService stockService, IMapper mapper, ILogger<StocksController> logger)
        {
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            try
            {
                var stockDTOs = await _stockService.GetAllStocksAsync();
                var stocks = _mapper.Map<List<Stock>>(stockDTOs);
                return View(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock index.");
                return View(new List<Stock>());
            }
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Stock ID not provided.");
            }

            // 1. Fetch the flat list of all product details for this stock.
            var stockDetailsFlatList = await _stockService.GetStockDetailsAsync(id.Value);

            if (stockDetailsFlatList == null || !stockDetailsFlatList.Any())
            {
                // If the stock exists but has nothing, we can still show a page.
                // First, check if the stock itself exists.
                var stock = await _stockService.GetStockByIdAsync(id.Value);
                if (stock == null)
                {
                    return NotFound($"Stock with ID {id.Value} not found.");
                }

                // If the stock exists but is empty, create a ViewModel with an empty product list.
                var emptyViewModel = new StockDetailsViewModel
                {
                    StockId = stock.StockId,
                    StockName = stock.StockName,
                    StockAddress = stock.StockAddress,
                    Products = new List<ProductInventoryGroup>() // Empty list
                };
                return View(emptyViewModel);
            }

            // 2. Group the flat list by ProductId to create our hierarchical structure.
            var groupedProducts = stockDetailsFlatList
                .Where(d => d.ProductId.HasValue) // Ensure we only process items that are linked to a product
                .GroupBy(d => d.ProductId.Value)
                .Select(group => new ProductInventoryGroup
                {
                    ProductId = group.Key,
                    ProductName = group.First().ProductName,
                    BrandName = group.First().BrandName,
                    // For each product group, create a list of its variations
                    Variations = group.Select(item => new VariationInventoryDetail
                    {
                        VariationId = item.VariationId.Value,
                        SizeName = item.SizeName,
                        ColorName = item.ColorName,
                        Quantity = item.Quantity ?? 0
                    }).ToList()
                }).ToList();

            // 3. Create the final ViewModel.
            var viewModel = new StockDetailsViewModel
            {
                StockId = id.Value,
                StockName = stockDetailsFlatList.First().StockName,
                StockAddress = stockDetailsFlatList.First().StockAddress,
                Products = groupedProducts
            };

            return View(viewModel);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            return View(new StockCreateDTO()); // Pass DTO to the view for strongly-typed form
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockCreateDTO stockDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // This assumes IStockService has a CreateStockAsync method
                    // that takes StockCreateDTO and handles mapping and saving.
                    var createdStockDto = await _stockService.CreateStockAsync(stockDto);
                    if (createdStockDto != null)
                    {
                        TempData["SuccessMessage"] = "Stock created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create stock. Please try again.");
                    }
                }
                catch (Exception ex) // Catch specific exceptions from service if any
                {
                    _logger.LogError(ex, "Error creating stock.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the stock.");
                }
            }
            return View(stockDto); // Return DTO to repopulate form with validation errors
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("Stock ID not provided.");
            }

            try
            {
                var stockDto = await _stockService.GetStockByIdAsync(id.Value);
                if (stockDto == null)
                {
                    return NotFound($"Stock with ID {id.Value} not found for editing.");
                }
                // Map StockDTO to StockUpdateDTO for the form
                var stockUpdateDto = _mapper.Map<StockUpdateDTO>(stockDto);
                return View(stockUpdateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving stock ID {id.Value} for edit.");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StockUpdateDTO stockDto)
        {
            if (id != stockDto.StockId) // Ensure ID from route matches ID in DTO
            {
                return BadRequest("Mismatched stock ID.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // This assumes IStockService has an UpdateStockAsync method
                    bool updated = await _stockService.UpdateStockAsync(id, stockDto);
                    if (updated)
                    {
                        TempData["SuccessMessage"] = "Stock updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Could be NotFound if service returns false because item didn't exist
                        ModelState.AddModelError(string.Empty, "Failed to update stock. It might have been deleted by another user.");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogWarning(ex, $"Concurrency error updating stock ID {id}.");
                    ModelState.AddModelError(string.Empty, "The stock was modified by another user. Please reload and try again.");
                    // To repopulate form with attempted values and allow resubmit:
                    // var currentStockDto = await _stockService.GetStockByIdAsync(id);
                    // if (currentStockDto != null) _mapper.Map(currentStockDto, stockDto); // Overwrite with current server values if preferred
                }
                catch (Exception ex) // Catch specific exceptions from service
                {
                    _logger.LogError(ex, $"Error updating stock ID {id}.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the stock.");
                }
            }
            return View(stockDto); // Return DTO to repopulate form with validation errors
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Stock ID not provided.");
            }
            try
            {
                var stockDto = await _stockService.GetStockByIdAsync(id.Value);
                if (stockDto == null)
                {
                    return NotFound($"Stock with ID {id.Value} not found for deletion.");
                }
                var stock = _mapper.Map<Stock>(stockDto); // View expects Stock entity
                return View(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving stock ID {id.Value} for delete confirmation.");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // This assumes IStockService has a DeleteStockAsync method
                bool deleted = await _stockService.DeleteStockAsync(id);
                if (deleted)
                {
                    TempData["SuccessMessage"] = "Stock deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Could not delete stock with ID {id}. It might have already been deleted.";
                }
            }
            catch (Exception ex) // Catch specific exceptions from service (e.g., if stock has dependencies)
            {
                _logger.LogError(ex, $"Error deleting stock ID {id}.");
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        // The StockExists method is typically a repository concern,
        // the service layer would handle "not found" logic.
        // private bool StockExists(int id)
        // {
        //     return _stockService.GetStockByIdAsync(id) != null; // Example if service has such a check
        // }
    }
}
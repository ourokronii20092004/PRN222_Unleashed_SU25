using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IProductService productService, ILogger<DetailsModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Product ID is required.";
                return RedirectToPage("./Index");
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id.Value);
                if (product == null)
                {
                    TempData["ErrorMessage"] = $"Product with ID {id} not found.";
                    return NotFound();
                }

                Product = product;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product details for ID {id}");
                TempData["ErrorMessage"] = "An error occurred while retrieving product details.";
                return RedirectToPage("./Index");
            }
        }
    }
}
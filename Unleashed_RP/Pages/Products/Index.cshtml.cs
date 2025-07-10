using BLL.Services.Interfaces;
using DAL.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageIndex { get; set; }

        [BindProperty(SupportsGet = true)]
        public int pageSize { get; set; } = 10;

        public IList<ProductListDTO> Product { get; set; } = new List<ProductListDTO>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage => pageIndex ?? 1;

        public IndexModel(IProductService productService, ILogger<IndexModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("OnGetAsync received: pageIndex={pageIndex}, pageSize={pageSize}, query='{query}'",
                pageIndex, pageSize, Query);

            try
            {
                pageSize = Math.Clamp(pageSize, 1, 100);
                Query = Query ?? string.Empty;

                var pagedResult = await _productService.GetProductsWithPagingAsync(CurrentPage, pageSize, Query);

                Product = pagedResult.Items;
                TotalCount = pagedResult.TotalCount;
                TotalPages = pagedResult.TotalPages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                ModelState.AddModelError("", "An error occurred.");
            }
        }
    }
}
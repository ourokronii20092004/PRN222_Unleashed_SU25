using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Services.Interfaces;
using DAL.DTOs.ProductDTOs;

namespace Unleashed_RP.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(IProductService productService, ILogger<IndexModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public IList<ProductListDTO> Product { get; set; } = new List<ProductListDTO>();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Query { get; set; } = string.Empty;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int page = 1, int pageSize = 10, string query = "")
        {
            _logger.LogInformation("OnGetAsync called with page: {Page}, pageSize: {PageSize}, query: '{Query}'", page, pageSize, query);
            try
            {
                Page = Math.Max(1, page);  
                PageSize = Math.Clamp(pageSize, 1, 100);  
                Query = query ?? string.Empty; 

                var pagedResult = await _productService.GetProductsWithPagingAsync(Page, PageSize, Query);

                Product = pagedResult.Items;
                TotalCount = pagedResult.TotalItems;
                TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
                

                if (TotalPages > 0 && Page > TotalPages)
                {
                    Page = TotalPages;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving products.");
            }
        }
    }
}
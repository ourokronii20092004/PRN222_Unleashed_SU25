using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.DTOs.ProductDTOs;
using BLL.Services.Interfaces;

namespace Unleashed_RP.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<ProductListDTO> Product { get; set; } = default!;
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Query { get; set; } = string.Empty;  

        public async Task OnGetAsync(int page = 1, int pageSize = 10, string query = "")
        {
            int skip = (page - 1) * pageSize;
            Query = query;

            var products = await _productService.GetAllProductsAsync();

            Product = products.Skip(skip).Take(pageSize).ToList();

            TotalCount = products.Count();
            Page = page;
            PageSize = pageSize;
        }
    }
}

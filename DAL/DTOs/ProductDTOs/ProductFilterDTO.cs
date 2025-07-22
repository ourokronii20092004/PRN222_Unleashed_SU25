using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProductDTOs
{
    public class ProductFilterDTO
    {
        // Paging
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        // Filtering
        public string? Query { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }

        // Sorting
        public string? SortBy { get; set; }
    }
}

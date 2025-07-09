using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class ProductBrowseViewModel
    {
        public PagedResult<BrowseableProductViewModel> PagedProducts { get; set; } = null!;
        public List<int> SelectedVariationIds { get; set; } = new List<int>(); // To maintain selection across pages
        public string? SearchTerm { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int StockIdToImportTo { get; set; } // To carry over which stock we're importing to
    }

    public class BrowseableProductViewModel
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? BrandName { get; set; }
        public List<BrowseableVariationViewModel> Variations { get; set; } = new List<BrowseableVariationViewModel>();
    }

    public class BrowseableVariationViewModel
    {
        public int VariationId { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
        public string? VariationImage { get; set; }
        public decimal? VariationPrice { get; set; }
    }

    // Helper for Paging (can be a generic class)
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

}

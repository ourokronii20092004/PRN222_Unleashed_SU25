using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class ImportQuantitiesViewModel
    {
        [Required]
        public int StockId { get; set; }
        public string? StockName { get; set; }

        [Required(ErrorMessage = "Provider is required for import.")]
        public int? ProviderId { get; set; }

        public List<VariationToImportViewModel> VariationsToImport { get; set; } = new List<VariationToImportViewModel>();

        // For dropdown
        public SelectList? Providers { get; set; }
    }

    public class VariationToImportViewModel
    {
        public int VariationId { get; set; }
        public string? ProductName { get; set; }
        public string? VariationDescription { get; set; }
        public string? VariationImage { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int? Quantity { get; set; }
    }
}
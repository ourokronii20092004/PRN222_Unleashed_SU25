using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.TransactionDTOs
{
    public class TransactionCardDTO
    {
        public int Id { get; set; }
        public string? VariationImage { get; set; }
        public string? ProductName { get; set; }
        public string? StockName { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
        public decimal? TransactionProductPrice { get; set; }
        public int? TransactionQuantity { get; set; }

        // Java uses LocalDate. C# EF Core typically maps SQL DATE to DateOnly (EF Core 6+)
        // or DateTime (older versions/configurations).
        // If your Transaction entity uses DateOnly?, this is fine.
        // If it uses DateTime? or DateTimeOffset?, adjust DTO accordingly for consistency.
        public DateOnly? TransactionDate { get; set; }
        public string? InchargeEmployeeUsername { get; set; }
        public string? ProviderName { get; set; }
    }
}
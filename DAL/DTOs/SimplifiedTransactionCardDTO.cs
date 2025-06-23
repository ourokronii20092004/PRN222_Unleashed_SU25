using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class SimplifiedTransactionCardDTO
    {
        public int TransactionId { get; set; }
        public Guid ProductId { get; set; } // Changed from String to Guid to match Product entity
        public string? VariationImage { get; set; }
        public string? ProductName { get; set; }
        public string? StockName { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? BrandName { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
        public decimal? TransactionProductPrice { get; set; }
        public int? TransactionQuantity { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public string? InchargeEmployeeUsername { get; set; }
        public string? ProviderName { get; set; }

        // Constructor can be useful if you plan to use it with EF Core's .Select(x => new DTO(...))
        // or if you want to maintain the Java style.
        // public SimplifiedTransactionCardDTO(
        //     int transactionId, Guid productId, string? variationImage, string? productName,
        //     string? stockName, string? transactionTypeName, string? brandName,
        //     string? sizeName, string? colorName, string? colorHexCode,
        //     decimal? transactionProductPrice, int? transactionQuantity, DateOnly? transactionDate,
        //     string? inchargeEmployeeUsername, string? providerName)
        // {
        //     TransactionId = transactionId;
        //     ProductId = productId;
        //     VariationImage = variationImage;
        //     ProductName = productName;
        //     StockName = stockName;
        //     TransactionTypeName = transactionTypeName;
        //     BrandName = brandName;
        //     SizeName = sizeName;
        //     ColorName = colorName;
        //     ColorHexCode = colorHexCode;
        //     TransactionProductPrice = transactionProductPrice;
        //     TransactionQuantity = transactionQuantity;
        //     TransactionDate = transactionDate;
        //     InchargeEmployeeUsername = inchargeEmployeeUsername;
        //     ProviderName = providerName;
        // }
    }
}
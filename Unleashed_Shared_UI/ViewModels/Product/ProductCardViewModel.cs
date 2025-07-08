using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.ViewModels.Product
{
    public class ProductCardViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductVariationImage { get; set; }
        public decimal ProductPrice { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public SaleInfo Sale { get; set; }

        public class SaleInfo
        {
            public decimal SaleValue { get; set; }
            public string SaleTypeName { get; set; } // "PERCENTAGE" or "FIXED AMOUNT"
        }
    }
}
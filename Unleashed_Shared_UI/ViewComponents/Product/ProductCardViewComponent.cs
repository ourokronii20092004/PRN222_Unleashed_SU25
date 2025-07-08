using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unleashed_Shared_UI.Utils;
using Unleashed_Shared_UI.ViewModels.Product;

namespace Unleashed_Shared_UI.ViewComponents.Product
{
    public class ProductCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ProductCardViewModel product)
        {
            decimal discountedPrice = product.ProductPrice;
            string saleText = string.Empty;

            if (product.Sale != null && product.Sale.SaleValue > 0)
            {
                if (product.Sale.SaleTypeName == "PERCENTAGE")
                {
                    discountedPrice = product.ProductPrice - product.ProductPrice * (product.Sale.SaleValue / 100);
                    saleText = $"Save {product.Sale.SaleValue}%";
                }
                else if (product.Sale.SaleTypeName == "FIXED AMOUNT")
                {
                    discountedPrice = product.ProductPrice - product.Sale.SaleValue;
                    saleText = $"Save {FormattingUtils.FormatPrice(product.Sale.SaleValue)}";
                }
            }

            // Pass the calculated values to the view using ViewBag
            ViewBag.DisplayPrice = FormattingUtils.FormatPrice(discountedPrice);
            ViewBag.OriginalPrice = FormattingUtils.FormatPrice(product.ProductPrice);
            ViewBag.SaleText = saleText;

            return View(product);
        }
    }
}

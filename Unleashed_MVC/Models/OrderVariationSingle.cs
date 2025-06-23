using System;
using System.Collections.Generic;

namespace Unleashed_MVC.Models;

public partial class OrderVariationSingle
{
    public Guid OrderId { get; set; }

    public int VariationSingleId { get; set; }

    public int? SaleId { get; set; }

    public decimal VariationPriceAtPurchase { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual VariationSingle VariationSingle { get; set; } = null!;
}

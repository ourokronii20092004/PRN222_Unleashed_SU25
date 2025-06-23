using System;
using System.Collections.Generic;

namespace Unleashed_MVC.Models;

public partial class DiscountType
{
    public int DiscountTypeId { get; set; }

    public string? DiscountTypeName { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}

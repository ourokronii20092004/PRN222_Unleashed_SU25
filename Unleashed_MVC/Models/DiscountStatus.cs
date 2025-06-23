using System;
using System.Collections.Generic;

namespace Unleashed_MVC.Models;

public partial class DiscountStatus
{
    public int DiscountStatusId { get; set; }

    public string? DiscountStatusName { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}

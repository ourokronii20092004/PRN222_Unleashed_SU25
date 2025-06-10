using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Cart
{
    public string UserId { get; set; } = null!;

    public int VariationId { get; set; }

    public int? CartQuantity { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Variation Variation { get; set; } = null!;
}

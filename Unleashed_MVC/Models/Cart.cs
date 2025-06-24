using System;
using System.Collections.Generic;

namespace Unleashed_MVC.Models;

public partial class Cart
{
    public Guid UserId { get; set; }

    public int VariationId { get; set; }

    public int? CartQuantity { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Variation Variation { get; set; } = null!;
}

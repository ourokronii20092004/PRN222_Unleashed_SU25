using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class SaleStatus
{
    public int SaleStatusId { get; set; }

    public string? SaleStatusName { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class SaleType
{
    public int SaleTypeId { get; set; }

    public string? SaleTypeName { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

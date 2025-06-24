using System;
using System.Collections.Generic;

namespace Unleashed_MVC.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDescription { get; set; }

    public string? CategoryImageUrl { get; set; }

    public DateTimeOffset? CategoryCreatedAt { get; set; }

    public DateTimeOffset? CategoryUpdatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

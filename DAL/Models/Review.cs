using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string? ProductId { get; set; }

    public string? UserId { get; set; }

    public string? OrderId { get; set; }

    public int? ReviewRating { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}

using DAL.Models;

namespace DAL.DTOs.CartDTOs;
public class CartDTO
{
    public Variation Variation { get; set; }
    public int Quantity { get; set; }
    public int StockQuantity { get; set; }
}

public class VariationDTO
{
    public int VariationId { get; set; }
    public Guid ProductId { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public string? VariationImage { get; set; } = string.Empty;
    public decimal? VariationPrice { get; set; }
    public string? ColorName { get; set; } = string.Empty;
    public string? SizeName { get; set; } = "Name";
}

using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class StockCreateDTO
    {
        [Required(ErrorMessage = "Stock name is required.")]
        [StringLength(255, ErrorMessage = "Stock name cannot exceed 255 characters.")]
        public string StockName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Stock address cannot exceed 500 characters.")]
        public string? StockAddress { get; set; }
    }
}
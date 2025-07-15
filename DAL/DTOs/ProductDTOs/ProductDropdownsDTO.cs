using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProductDTOs
{
    public class ProductDropdownsDTO
    {
        public List<Brand> Brands { get; set; } = new List<Brand>();
        public List<ProductStatus> Statuses { get; set; } = new List<ProductStatus>();
        public List<Size> Sizes { get; set; } = new List<Size>();
        public List<Color> Colors { get; set; } = new List<Color>();
        public List<Category> Categories { get; set; }
    }
}

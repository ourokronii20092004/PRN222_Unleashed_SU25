using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class SearchBrandDTO
    {
        public string? BrandName { get; set; }
        public string? BrandDescription { get; set; }

        // Constructor can be added if desired, but often not necessary for DTOs in C#
        // public SearchBrandDTO(string? brandName, string? brandDescription)
        // {
        //     BrandName = brandName;
        //     BrandDescription = brandDescription;
        // }
    }
}
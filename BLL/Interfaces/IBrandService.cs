using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.Models;

namespace BLL.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDTO>> GetAllBrandsWithQuantityAsync();
        Task<BrandDTO?> GetBrandByIdAsync(int id);
        Task<List<SearchBrandDTO>> SearchBrandsAsync();

        Task<BrandDTO> CreateBrandAsync(BrandCreateDTO brandDto);
        Task<BrandDTO?> UpdateBrandAsync(int brandId, BrandUpdateDTO brandDto);
        Task<bool> DeleteBrandAsync(int brandId);
    }
}
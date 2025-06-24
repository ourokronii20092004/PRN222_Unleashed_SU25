using DAL.DTOs.BrandDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task<Brand> AddAsync(Brand brand);
        Task UpdateAsync(Brand brand); // EF Core tracks changes, so often no need to return the entity
        Task DeleteAsync(Brand brand);

        Task<bool> ExistsByNameAsync(string name, int? excludeBrandId = null);
        Task<bool> ExistsByWebsiteUrlAsync(string url, int? excludeBrandId = null);
        Task<List<BrandDTO>> FindAllBrandsWithQuantityAsync();

        Task<int> SaveChangesAsync(); // Useful for explicit save control from service
    }
}
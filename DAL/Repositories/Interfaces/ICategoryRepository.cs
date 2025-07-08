using DAL.DTOs.BrandDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category); // EF Core tracks changes, so often no need to return the entity
        Task DeleteAsync(Category category);
        Task<int> SaveChangesAsync(); // Useful for explicit save control from service
    }
}

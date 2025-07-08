using DAL.DTOs.CategoryDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        //Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO);
        Task<bool> DeleteCategoryAsync(int id);
        Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryUpdateDTO);

    }
}

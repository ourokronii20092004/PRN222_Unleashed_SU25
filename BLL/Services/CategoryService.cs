using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        public ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _categoryRepo.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            await _categoryRepo.Delete(category);
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return _categoryRepo.GetAllAsync();
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            return _categoryRepo.GetByIdAsync(id);
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDescription = category.CategoryDescription;
            existingCategory.CategoryImageUrl = category.CategoryImageUrl;
            await _categoryRepo.Update(existingCategory);
        }
    }
}

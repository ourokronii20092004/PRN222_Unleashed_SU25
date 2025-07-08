using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.BrandDTOs;
using DAL.DTOs.CategoryDTOs;
using DAL.Models;
using DAL.Repositories;
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
        public IProductRepository _productRepo;
        public IBrandRepository _brandRepo;
        public IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper, IProductRepository productRepo, IBrandRepository brandRepo)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _productRepo = productRepo;
            _brandRepo = brandRepo;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO)
        {
            var category = _mapper.Map<Category>(categoryCreateDTO);
            category.CategoryId = new();
            category.CategoryCreatedAt = DateTime.Now;
            category.CategoryUpdatedAt = DateTime.Now;

            var createdCategory = await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(createdCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            if (await _productRepo.ExistsByCategoryIdAsync(id))
            {
                throw new InvalidOperationException("Cannot delete category because it has linked products.");
            }


            await _categoryRepo.DeleteAsync(category);
            await _categoryRepo.SaveChangesAsync();
            return true;
        }

        //public async Task<List<Category>> GetAllCategoriesAsync()
        //{
        //    return await _categoryRepo.GetAllAsync();
        //}

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            _mapper.Map(categoryUpdateDTO, existingCategory);
            existingCategory.CategoryUpdatedAt = DateTime.Now;
            await _categoryRepo.UpdateAsync(existingCategory);
            await _categoryRepo.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(existingCategory);
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            return _mapper.Map<List<CategoryDTO>>(await _categoryRepo.GetAllAsync());
        }
    }
}

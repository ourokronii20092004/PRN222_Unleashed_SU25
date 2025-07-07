using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.BrandDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IProductRepository productRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<BrandDTO?> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return _mapper.Map<BrandDTO?>(brand);
        }

        public async Task<List<SearchBrandDTO>> SearchBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return _mapper.Map<List<SearchBrandDTO>>(brands);
        }


        public async Task<BrandDTO> CreateBrandAsync(BrandCreateDTO brandDto)
        {
            if (await _brandRepository.ExistsByNameAsync(brandDto.BrandName))
            {
                throw new InvalidOperationException($"Brand already exists with name: {brandDto.BrandName}");
            }
            if (await _brandRepository.ExistsByWebsiteUrlAsync(brandDto.BrandWebsiteUrl))
            {
                throw new InvalidOperationException($"Brand already exists with website URL: {brandDto.BrandWebsiteUrl}");
            }

            var brand = _mapper.Map<Brand>(brandDto);
            brand.BrandCreatedAt = DateTimeOffset.UtcNow;
            brand.BrandUpdatedAt = DateTimeOffset.UtcNow;

            var createdBrand = await _brandRepository.AddAsync(brand);
            await _brandRepository.SaveChangesAsync();
            return _mapper.Map<BrandDTO>(createdBrand);
        }

        public async Task<BrandDTO?> UpdateBrandAsync(int brandId, BrandUpdateDTO brandDto)
        {
            var existingBrand = await _brandRepository.GetByIdAsync(brandId);
            if (existingBrand == null)
            {
                return null;
            }

            if (existingBrand.BrandName != brandDto.BrandName &&
                await _brandRepository.ExistsByNameAsync(brandDto.BrandName, excludeBrandId: brandId))
            {
                throw new InvalidOperationException($"Another brand already exists with name: {brandDto.BrandName}");
            }
            if (existingBrand.BrandWebsiteUrl != brandDto.BrandWebsiteUrl &&
                await _brandRepository.ExistsByWebsiteUrlAsync(brandDto.BrandWebsiteUrl, excludeBrandId: brandId))
            {
                throw new InvalidOperationException($"Another brand already exists with website URL: {brandDto.BrandWebsiteUrl}");
            }

            _mapper.Map(brandDto, existingBrand);
            existingBrand.BrandUpdatedAt = DateTimeOffset.UtcNow;

            await _brandRepository.UpdateAsync(existingBrand);
            await _brandRepository.SaveChangesAsync();
            return _mapper.Map<BrandDTO>(existingBrand);
        }

        public async Task<bool> DeleteBrandAsync(int brandId)
        {
            var brand = await _brandRepository.GetByIdAsync(brandId);
            if (brand == null)
            {
                return false;
            }

            if (await _productRepository.ExistsByBrandAsync(brandId))
            {
                throw new InvalidOperationException("Cannot delete brand because it has linked products.");
            }

            await _brandRepository.DeleteAsync(brand);
            await _brandRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<BrandDTO>> GetAllBrandsWithQuantityAsync()
        {
            return await _brandRepository.FindAllBrandsWithQuantityAsync();
        }

    }
}

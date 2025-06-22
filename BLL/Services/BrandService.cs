using AutoMapper;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO;
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
            var brands = await _brandRepository.GetAllAsync(); // Gets all Brand entities
            return _mapper.Map<List<SearchBrandDTO>>(brands); // Maps entities to SearchBrandDTO
        }


        public async Task<BrandDTO> CreateBrandAsync(BrandCreateDTO brandDto)
        {
            // Validation (from Java service)
            if (await _brandRepository.ExistsByNameAsync(brandDto.BrandName))
            {
                throw new InvalidOperationException($"Brand already exists with name: {brandDto.BrandName}");
            }
            if (await _brandRepository.ExistsByWebsiteUrlAsync(brandDto.BrandWebsiteUrl))
            {
                throw new InvalidOperationException($"Brand already exists with website URL: {brandDto.BrandWebsiteUrl}");
            }
            // Basic property validation (null/empty) is typically handled by BrandCreateDTO annotations + MVC model state

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
                // throw new NotFoundCustomException($"Brand not found with id: {brandId}");
                return null;
            }

            // Validation (from Java service)
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

            _mapper.Map(brandDto, existingBrand); // Apply updates from DTO to entity
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
                return false; // Or throw NotFound
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
            // This directly calls the repository method that executes the complex query
            return await _brandRepository.FindAllBrandsWithQuantityAsync();
        }

        // The Java toOffsetDateTime helper is generally not needed if:
        // 1. Your database stores timestamps in a way that EF Core can map directly to DateTimeOffset.
        // 2. The custom query in FindAllBrandsWithQuantityAsync returns types that EF Core can map.
        //    DateTimeOffset is generally well-supported.
    }
}

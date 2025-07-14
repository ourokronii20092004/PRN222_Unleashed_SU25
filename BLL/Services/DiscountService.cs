using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.DiscountDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository discountRepo, IMapper mapper, ILogger<DiscountService> logger)
        {
            _discountRepo = discountRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<DiscountDTO>> GetAllDiscountsAsync()
        {
            var discounts = await _discountRepo.GetAllAsync();
            return _mapper.Map<List<DiscountDTO>>(discounts);
        }

        public async Task<DiscountDTO?> GetDiscountByIdAsync(int id)
        {
            var discount = await _discountRepo.GetByIdAsync(id);
            return _mapper.Map<DiscountDTO>(discount);
        }

        public async Task<DiscountDTO> CreateDiscountAsync(DiscountCreateDTO discountDto)
        {
            if (await _discountRepo.CodeExistsAsync(discountDto.DiscountCode))
            {
                throw new InvalidOperationException($"Discount code '{discountDto.DiscountCode}' already exists.");
            }

            var discount = _mapper.Map<Discount>(discountDto);
            discount.DiscountCreatedAt = DateTimeOffset.UtcNow;
            discount.DiscountUsageCount = 0; 

            await _discountRepo.AddAsync(discount);
            await _discountRepo.SaveChangesAsync();
            return _mapper.Map<DiscountDTO>(discount);
        }

        public async Task UpdateDiscountAsync(int id, DiscountUpdateDTO discountDto)
        {
            var existingDiscount = await _discountRepo.GetByIdAsync(id);
            if (existingDiscount == null)
            {
                throw new KeyNotFoundException($"Discount with ID {id} not found.");
            }

            if (await _discountRepo.CodeExistsAsync(discountDto.DiscountCode, id))
            {
                throw new InvalidOperationException($"Discount code '{discountDto.DiscountCode}' already exists.");
            }

            _mapper.Map(discountDto, existingDiscount);
            existingDiscount.DiscountUpdatedAt = DateTimeOffset.UtcNow;

            await _discountRepo.UpdateAsync(existingDiscount);
            await _discountRepo.SaveChangesAsync();
        }

        public async Task DeleteDiscountAsync(int id)
        {
            var discount = await _discountRepo.GetByIdAsync(id);
            if (discount == null)
            {
                throw new KeyNotFoundException($"Discount with ID {id} not found.");
            }

            await _discountRepo.DeleteAsync(discount);
            await _discountRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDiscountStatusesAsync()
        {
            var statuses = await _discountRepo.GetAllStatusesAsync();
            return statuses.Select(s => new SelectListItem
            {
                Value = s.DiscountStatusId.ToString(),
                Text = s.DiscountStatusName
            }).ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetDiscountTypesAsync()
        {
            var types = await _discountRepo.GetAllTypesAsync();
            return types.Select(t => new SelectListItem
            {
                Value = t.DiscountTypeId.ToString(),
                Text = t.DiscountTypeName
            }).ToList();
        }

        public async Task<DiscountUpdateDTO?> GetDiscountForUpdateAsync(int id)
        {
            var discount = await _discountRepo.GetByIdAsync(id);
            if (discount == null) return null;
            return _mapper.Map<DiscountUpdateDTO>(discount);
        }
    }
}
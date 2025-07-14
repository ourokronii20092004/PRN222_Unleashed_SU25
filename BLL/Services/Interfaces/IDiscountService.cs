using DAL.DTOs.DiscountDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountDTO>> GetAllDiscountsAsync();
        Task<DiscountDTO?> GetDiscountByIdAsync(int id);
        Task<DiscountDTO> CreateDiscountAsync(DiscountCreateDTO discountDto);
        Task UpdateDiscountAsync(int id, DiscountUpdateDTO discountDto);
        Task DeleteDiscountAsync(int id);

        Task<IEnumerable<SelectListItem>> GetDiscountStatusesAsync();
        Task<IEnumerable<SelectListItem>> GetDiscountTypesAsync();
        Task<DiscountUpdateDTO?> GetDiscountForUpdateAsync(int id);
    }
}

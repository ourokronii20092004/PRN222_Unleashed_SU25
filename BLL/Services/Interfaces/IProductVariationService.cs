using DAL.DTOs;
using DAL.Models;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductVariationService
    {
        Task<Variation?> FindByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
        Task<Variation> UpdateProductVariationAsync(int variationId, ProductDTO.ProductVariationDTO variationDTO);
    }
}

using DAL.DTOs.VariationDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IVariationService
    {

        Task<List<VariationDetailDTO>> GetVariationDetailsForProductsAsync(List<Guid> productIds);

        Task<List<StockVariation>> GetStockVariationsByVariationIdAsync(int variationId);

    }
}

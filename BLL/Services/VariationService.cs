using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.VariationDTOs;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class VariationService : IVariationService
    {
        private readonly IVariationRepository _variationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VariationService> _logger;

        public VariationService(IVariationRepository variationRepository, IMapper mapper, ILogger<VariationService> logger)
        {
            _variationRepository = variationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VariationDetailDTO>> GetVariationDetailsForProductsAsync(List<Guid> productIds)
        {
            try
            {
                var variations = await _variationRepository.GetVariationsForProductsAsync(productIds);
                return _mapper.Map<List<VariationDetailDTO>>(variations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching variation details for products.");
                throw;
            }
        }
    }
}
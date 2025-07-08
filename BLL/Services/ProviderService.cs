using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.ProviderDTOs;
using DAL.Repositories.Interfaces;
using global::BLL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderService> _logger;

        public ProviderService(IProviderRepository providerRepository, IMapper mapper, ILogger<ProviderService> logger)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProviderBareboneDTO>> GetAllProvidersAsync()
        {
            try
            {
                var providers = await _providerRepository.GetAllAsync();

                return _mapper.Map<List<ProviderBareboneDTO>>(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all providers.");
                throw;
            }
        }
    }
}
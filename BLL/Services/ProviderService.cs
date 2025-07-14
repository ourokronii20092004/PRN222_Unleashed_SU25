using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.ProviderDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Getting all providers.");
            var providers = await _providerRepository.GetAllAsync();
            return _mapper.Map<List<ProviderBareboneDTO>>(providers);
        }

        public async Task<ProviderDetailsDTO?> GetProviderByIdAsync(int id)
        {
            _logger.LogInformation("Getting provider by id: {ProviderId}", id);
            var provider = await _providerRepository.GetByIdAsync(id);
            return _mapper.Map<ProviderDetailsDTO>(provider);
        }

        public async Task<ProviderDetailsDTO> CreateProviderAsync(ProviderCreateDTO providerDto)
        {
            _logger.LogInformation("Creating a new provider with name: {ProviderName}", providerDto.ProviderName);
            var provider = _mapper.Map<Provider>(providerDto);

            // Set giá trị mặc định nếu cần
            provider.ProviderCreatedAt = DateTimeOffset.UtcNow;

            await _providerRepository.AddAsync(provider);
            await _providerRepository.SaveChangesAsync();

            return _mapper.Map<ProviderDetailsDTO>(provider);
        }

        public async Task<ProviderEditDTO?> GetProviderForEditAsync(int id)
        {
            _logger.LogInformation("Getting provider for edit with id: {ProviderId}", id);
            var provider = await _providerRepository.GetByIdAsync(id);
            return _mapper.Map<ProviderEditDTO>(provider);
        }

        public async Task UpdateProviderAsync(int id, ProviderEditDTO providerDto)
        {
            _logger.LogInformation("Updating provider with id: {ProviderId}", id);
            var existingProvider = await _providerRepository.GetByIdAsync(id);
            if (existingProvider == null)
            {

                throw new KeyNotFoundException($"Provider with id {id} not found.");
            }


            _mapper.Map(providerDto, existingProvider);
            existingProvider.ProviderUpdatedAt = DateTimeOffset.UtcNow;

            await _providerRepository.UpdateAsync(existingProvider);
            await _providerRepository.SaveChangesAsync();
        }

        public async Task DeleteProviderAsync(int id)
        {
            _logger.LogInformation("Deleting provider with id: {ProviderId}", id);
            var provider = await _providerRepository.GetByIdAsync(id);
            if (provider == null)
            {
                throw new KeyNotFoundException($"Provider with id {id} not found.");
            }

            await _providerRepository.DeleteAsync(provider);
            await _providerRepository.SaveChangesAsync();
        }
    }
}
using DAL.DTOs.ProviderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProviderService
    {

        Task<List<ProviderBareboneDTO>> GetAllProvidersAsync();

        Task<ProviderDetailsDTO?> GetProviderByIdAsync(int id);

        Task<ProviderDetailsDTO> CreateProviderAsync(ProviderCreateDTO providerDto);

        Task<ProviderEditDTO?> GetProviderForEditAsync(int id);

        Task UpdateProviderAsync(int id, ProviderEditDTO providerDto);

        Task DeleteProviderAsync(int id);
    }
}
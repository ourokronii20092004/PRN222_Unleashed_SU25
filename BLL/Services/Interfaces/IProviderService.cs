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
        /// <summary>
        /// Retrieves a list of all providers formatted as barebone DTOs.
        /// </summary>
        /// <returns>A list of ProviderBareboneDTOs suitable for UI consumption.</returns>
        Task<List<ProviderBareboneDTO>> GetAllProvidersAsync();
    }
}
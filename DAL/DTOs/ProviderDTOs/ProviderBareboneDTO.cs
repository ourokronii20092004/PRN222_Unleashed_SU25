using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProviderDTOs
{
    public class ProviderBareboneDTO
    {
        public int ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderImageUrl { get; set; }
    }
}

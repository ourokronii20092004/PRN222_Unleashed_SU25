using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProviderDTOs
{
    public class ProviderDetailsDTO
    {
        public int ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderImageUrl { get; set; }
        public string? ProviderEmail { get; set; }
        public string? ProviderPhone { get; set; }
        public string? ProviderAddress { get; set; }
        public DateTimeOffset? ProviderCreatedAt { get; set; }
        public DateTimeOffset? ProviderUpdatedAt { get; set; }
    }
}

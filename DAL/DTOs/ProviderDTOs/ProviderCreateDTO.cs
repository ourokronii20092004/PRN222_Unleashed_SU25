using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProviderDTOs
{
    public class ProviderCreateDTO
    {
        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc.")]
        [StringLength(100)]
        public string ProviderName { get; set; }

        public string? ProviderImageUrl { get; set; }

        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string? ProviderEmail { get; set; }

        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
        public string? ProviderPhone { get; set; }

        public string? ProviderAddress { get; set; }
    }
}

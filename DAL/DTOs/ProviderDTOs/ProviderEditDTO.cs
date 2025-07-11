using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProviderDTOs
{
    public class ProviderEditDTO
    {
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Tên nhà cung cấp phải có từ 6 đến 100 ký tự.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Tên nhà cung cấp không được chứa ký tự đặc biệt.")]
        public string ProviderName { get; set; }

        public string? ProviderImageUrl { get; set; }

        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string? ProviderEmail { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải là 10 hoặc 11 chữ số và không chứa ký tự khác.")]
        public string? ProviderPhone { get; set; }

        public string? ProviderAddress { get; set; }
    }
}

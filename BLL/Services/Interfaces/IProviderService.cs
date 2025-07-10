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
        /// Lấy danh sách tất cả nhà cung cấp (dạng rút gọn).
        /// </summary>
        Task<List<ProviderBareboneDTO>> GetAllProvidersAsync();

        /// <summary>
        /// Lấy thông tin chi tiết của một nhà cung cấp bằng ID.
        /// </summary>
        Task<ProviderDetailsDTO?> GetProviderByIdAsync(int id);

        /// <summary>
        /// Tạo một nhà cung cấp mới.
        /// </summary>
        Task<ProviderDetailsDTO> CreateProviderAsync(ProviderCreateDTO providerDto);

        /// <summary>
        /// Lấy DTO để chỉnh sửa nhà cung cấp.
        /// </summary>
        Task<ProviderEditDTO?> GetProviderForEditAsync(int id);

        /// <summary>
        /// Cập nhật thông tin một nhà cung cấp.
        /// </summary>
        Task UpdateProviderAsync(ProviderEditDTO providerDto);

        /// <summary>
        /// Xóa một nhà cung cấp bằng ID.
        /// </summary>
        Task DeleteProviderAsync(int id);
    }
}
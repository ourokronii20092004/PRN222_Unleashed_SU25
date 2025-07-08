using DAL.DTOs.NotificationDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface INotificationService
    {
        public Task<IEnumerable<NotificationDetailDTO>> GetAllNotificationsAsync();
        public Task<bool> AddNotificationAsync(NotificationCreateDTO notification);
        public Task<bool> EditNotificationAsync(NotificationDetailDTO notificationDetailDTO);
        public Task<bool> RemoveNotificationAsync(int id);
        public Task<NotificationDetailDTO> GetNotificationByIdAsync(int id);
    }
}

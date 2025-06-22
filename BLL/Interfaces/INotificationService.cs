using DAL.DTOs.NotificationDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INotificationService
    {
        public Task<IEnumerable<NotificationDetailDTO>> GetAllNotificationsAsync();
        public Task<bool> AddNotificationAsync(Notification notification, IEnumerable<string> usernames);
        public Task<bool> EditNotificationAsync(Notification notification, IEnumerable<string> usernames);
        public Task<bool> RemoveNotificationAsync(int id);
        public Task<Notification> GetNotificationByIdAsync(int id);
    }
}

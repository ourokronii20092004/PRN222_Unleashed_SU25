using DAL.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface INotificationUserService
    {
        Task<IEnumerable<NotificationUserDetailDTO>> GetNotificationUserListAsync(string username);
        Task<bool> SetViewedUserNotification(string username, int notificationId);
        Task<bool> DeleteUserNotification(string username, int notificationId);
    }
}

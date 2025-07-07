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
        Task SetViewedUserNotification(string username, int notificationId);
        Task DeleteUserNotification(string username, int notificationId);
    }
}

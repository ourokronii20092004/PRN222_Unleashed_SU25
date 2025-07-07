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
        Task<(IEnumerable<NotificationUserDetailDTO>, int total)> GetNotificationUserListAsync(string username);
        Task SetViewedUserNotification(int notificationId);
        Task DeleteUserNotification(int notificationId);
    }
}

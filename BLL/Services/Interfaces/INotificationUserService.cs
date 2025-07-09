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
        Task<IEnumerable<NotificationUserDetailDTO>?> GetNotificationUserListAsync(string username);
        Task<(IEnumerable<NotificationUserDetailDTO>, int totalAmount)> GetNotificationUserListAsync(string username,string SearchString,int currentPage,int pageSize);
        Task<bool> SetViewedUserNotification(string username, int notificationId);
        Task<bool> DeleteUserNotification(string username, int notificationId);
    }
}

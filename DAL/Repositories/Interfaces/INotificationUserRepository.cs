using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface INotificationUserRepository: IGenericRepository<NotificationUser, (int, Guid)>
    {
        Task<IEnumerable<Notification>> GetNotificationByUserId (Guid userId);
        Task<IEnumerable<NotificationUser>> GetNotificationUserByNotificationId (int NotificationId);

        Task AddRangeAsync (IEnumerable<NotificationUser> notificationUsers, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync (IEnumerable<NotificationUser> notificationUsers,CancellationToken cancellationToken = default);
    }
}

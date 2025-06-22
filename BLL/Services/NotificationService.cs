
using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationUserRepository _notificationUserRepository;
        private readonly IAccountRepository _accountRepository;
        
        public NotificationService(INotificationRepository notificationRepository, INotificationUserRepository notificationUser, IAccountRepository accountRepository) 
        {
            _notificationRepository = notificationRepository;
            _notificationUserRepository = notificationUser;
            _accountRepository = accountRepository;
        }

        public async Task<bool> AddNotificationAsync(Notification notification, IEnumerable<User> users)
        {
            try
            {
                await _notificationRepository.AddAsync(notification);
                if (users != null && users.Any())
                {
                    foreach (var user in users)
                    {
                        await _notificationUserRepository.AddAsync(new NotificationUser
                        {
                            User = user,
                            Notification = notification,
                        });
                    }
                }
                return true;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> EditNotificationAsync(Notification notification, IEnumerable<User> users)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return _notificationRepository.GetAllAsync();
        }

        public Task<Notification> GetNotificationByIdAsync(int id)
        {
            return _notificationRepository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveNotificationAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            try
            {
                var notificationUsers = await _notificationUserRepository.GetNotificationUserByNotificationId(id);
                if (notificationUsers != null)
                {
                    foreach (var notificationUser in notificationUsers)
                    {
                        notificationUser.IsNotificationDeleted = true;
                        await _notificationUserRepository.Update(notificationUser);
                    }
                    return true;
                } 
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

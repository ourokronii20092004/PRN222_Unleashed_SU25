
using BLL.Interfaces;
using DAL.DTOs.NotificationDTOs;
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

        public async Task<bool> AddNotificationAsync(Notification notification, IEnumerable<string> usernames)
        {
            try
            {
                notification.NotificationCreatedAt = DateTimeOffset.UtcNow;
                notification.NotificationUpdatedAt = DateTimeOffset.UtcNow;
                notification.UserIdSenderNavigation = await _accountRepository.GetByIdAsync(notification.UserIdSender);
               
                await _notificationRepository.AddAsync(notification);
                if (usernames != null && usernames.Any())
                {
                    foreach (var username in usernames)
                    {
                        await _notificationUserRepository.AddAsync(new NotificationUser
                        {
                            User = await _accountRepository.GetByUsernameAsync(username),
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

        public async Task<bool> EditNotificationAsync(Notification notification, IEnumerable<string> usernames)
        {
            try
            {
                if (usernames != null && usernames.Any() && notification != null)
                {
                    foreach (var username in usernames)
                    {
                        await _notificationUserRepository.AddAsync(new NotificationUser
                        {
                            User = await _accountRepository.GetByUsernameAsync(username),
                            Notification = notification,
                        });
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

        public async Task<IEnumerable<NotificationDetailDTO>> GetAllNotificationsAsync()
        {
            IEnumerable<Notification> notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(n => NotificationDetailDTO.FromNotification(n));
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _notificationRepository.GetByIdAsync(id);
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

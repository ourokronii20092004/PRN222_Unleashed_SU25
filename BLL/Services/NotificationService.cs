using AutoMapper;
using BLL.Services.Interfaces;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public NotificationService(INotificationRepository notificationRepository, INotificationUserRepository notificationUser, IUserRepository accountRepository, IMapper mapper) 
        {
            _notificationRepository = notificationRepository;
            _notificationUserRepository = notificationUser;
            _userRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddNotificationAsync(NotificationCreateDTO createNotification)
        {
            try
            {
                var notification = _mapper.Map<Notification>(createNotification);
                notification.NotificationCreatedAt = DateTimeOffset.UtcNow;
                notification.NotificationUpdatedAt = DateTimeOffset.UtcNow;
                notification.UserIdSenderNavigation = await _userRepository.GetByIdAsync(notification.UserIdSender);
               
                await _notificationRepository.AddAsync(notification);
                IEnumerable<string> usernames= createNotification.Receivers;
                if (usernames != null && usernames.Any())
                {
                    IEnumerable<User> users = await _userRepository.FindAsync(u => usernames.Contains(u.UserUsername));
                    if (users != null && users.Any())
                    {
                        IEnumerable<NotificationUser> notificationUsers = users.Select(u => new NotificationUser
                        {
                            UserId = u.UserId,
                            NotificationId = notification.NotificationId,
                            User = u,
                            Notification = notification,
                        });
                        await _notificationUserRepository.AddRangeAsync(notificationUsers);
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

        public async Task<bool> EditNotificationAsync(int id, IEnumerable<string> usernames)
        {
            try
            {
                ArgumentOutOfRangeException.ThrowIfNegative(id);
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (usernames != null && usernames.Any() && notification != null)
                {
                    IEnumerable<User> users = await _userRepository.FindAsync(u => usernames.Contains(u.UserUsername));
                    if (users != null && users.Any())
                    {
                        IEnumerable<NotificationUser> notificationUsers = users.Select(u => new NotificationUser
                        {
                            UserId = u.UserId,
                            NotificationId = notification.NotificationId,
                            User = u,
                            Notification = notification,
                        });
                        await _notificationUserRepository.AddRangeAsync(notificationUsers);
                        return true;
                    }
                    return false;
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
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
            try
            {
                var notificationUsers = await _notificationUserRepository.GetNotificationUserByNotificationId(id);
                if (notificationUsers != null && notificationUsers.Any())
                {
                    foreach (var notificationUser in notificationUsers)
                    {
                        notificationUser.IsNotificationDeleted = true;                       
                    }
                    await _notificationUserRepository.UpdateRangeAsync(notificationUsers);
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

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
                var sender = await _userRepository.GetByUsernameAsync(createNotification.UsernameSender);
                ArgumentNullException.ThrowIfNull(sender, nameof(sender));
                ArgumentNullException.ThrowIfNull(createNotification, nameof(createNotification));
                var notification = _mapper.Map<Notification>(createNotification);
                notification.UserIdSender = sender.UserId;
                notification.UserIdSenderNavigation = sender;
                notification.NotificationCreatedAt = DateTimeOffset.UtcNow;
                notification.NotificationUpdatedAt = DateTimeOffset.UtcNow;

                await _notificationRepository.AddAsync(notification);
                if (!createNotification.IsNotificationDraft)
                {
                   await AddNotificationUsersForPublishedNotification(notification);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public async Task<bool> EditNotificationAsync(NotificationDetailDTO notificationDetailDTO)
        {
         try
            {
                ArgumentNullException.ThrowIfNull(notificationDetailDTO, nameof(notificationDetailDTO));
                var notification = await _notificationRepository.GetByIdAsync(notificationDetailDTO.NotificationId);
                if (notification != null && notification.IsNotificationDraft.GetValueOrDefault(true)) {
                    notification = _mapper.Map(notificationDetailDTO,notification);
                    notification.NotificationUpdatedAt = DateTime.UtcNow;
                    await _notificationRepository.Update(notification);
                    if(!notification.IsNotificationDraft.GetValueOrDefault(true))
                    await AddNotificationUsersForPublishedNotification(notification);
                return true;    
                } else return false;
            } catch
            {
                return false;
            }

        }

        public async Task<IEnumerable<NotificationDetailDTO>> GetAllNotificationsAsync()
        {
            IEnumerable<Notification> notifications = await _notificationRepository.FindAsync(noti => noti.IsNotificationDraft != null);
            return _mapper.Map<IEnumerable<NotificationDetailDTO>>(notifications);
        }

        public async Task<(IEnumerable<NotificationDetailDTO>, int totalAmount)> GetAllNotificationsAsync(string? SearchString, int currentPage, int pageSize)
        {
            IEnumerable<Notification> notifications;
            if (!string.IsNullOrEmpty(SearchString))
            {
                notifications = await _notificationRepository.FindAsync(noti => noti.IsNotificationDraft != null && noti.NotificationTitle.Contains(SearchString));
            }
            else
            {
                notifications = await _notificationRepository.FindAsync(noti => noti.IsNotificationDraft != null);
            }
            int totalAmount = notifications.Count();
            return (_mapper
                .Map<IEnumerable<NotificationDetailDTO>>
                (notifications
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)), totalAmount);
        }

        public async Task<NotificationDetailDTO> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            return _mapper.Map<NotificationDetailDTO>(notification);
        }

        public async Task<bool> RemoveNotificationAsync(int id)
        {
            
            try
            {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
                var notication = await _notificationRepository.GetByIdAsync(id);
                notication.IsNotificationDraft = null;
                await _notificationRepository.Update(notication);
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
        private async Task AddNotificationUsersForPublishedNotification(Notification notification)
        {
            IEnumerable<User> users = await _userRepository.FindAsync(u => u.RoleId == 2);
            if (users.Any()) 
            {
                IEnumerable<NotificationUser> notificationUsers = users.Select(u => new NotificationUser
                {
                    UserId = u.UserId,
                    NotificationId = notification.NotificationId,
                    // User = u, // Only set if you specifically need the navigation object to be tracked immediately
                    // Notification = notification, // Only set if you specifically need the navigation object to be tracked immediately
                });
                await _notificationUserRepository.AddRangeAsync(notificationUsers);
            }
        }
    }

}

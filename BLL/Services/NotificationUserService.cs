using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NotificationUserService : INotificationUserService
    {

        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationUserRepository _notificationUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public NotificationUserService(INotificationRepository notificationRepository, INotificationUserRepository notificationUserRepository, IUserRepository userRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _notificationUserRepository = notificationUserRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteUserNotification(string username, int notificationId)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                ArgumentNullException.ThrowIfNull(user, nameof(username));
                var notificationUser = await _notificationUserRepository.GetByIdAsync((notificationId, user.UserId));
                if (notificationUser != null)
                {
                    notificationUser.IsNotificationDeleted = true;
                    await _notificationUserRepository.Update(notificationUser);
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                return false;
            }
            
        }

        public async Task<IEnumerable<NotificationUserDetailDTO>?> GetNotificationUserListAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                ArgumentNullException.ThrowIfNull(user, nameof(username));
                var NotificationUsers = await _notificationUserRepository.FindAsync(nu => nu.UserId == user.UserId && nu.IsNotificationDeleted == false);
                return _mapper
                    .Map<IEnumerable<NotificationUserDetailDTO>>(NotificationUsers
                    .OrderByDescending(nu => nu.IsNotificationViewed)
                    .ThenByDescending(nu => nu.Notification.NotificationCreatedAt));
            }
            catch
            {
                return null;
            }
        }

        public async Task<(IEnumerable<NotificationUserDetailDTO>, int totalAmount)> GetNotificationUserListAsync(string username, string SearchString, int currentPage, int pageSize)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                ArgumentNullException.ThrowIfNull(user, nameof(username));
                IEnumerable<NotificationUser> notificationUsers;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    notificationUsers = await _notificationUserRepository.FindAsync(nu => nu.UserId == user.UserId && nu.IsNotificationDeleted == false && nu.Notification.NotificationTitle.Contains(SearchString));
                }
                else
                {
                    notificationUsers = await _notificationUserRepository.FindAsync(nu => nu.UserId == user.UserId && nu.IsNotificationDeleted == false);
                }
                int totalAmount = notificationUsers.Count();
                return (_mapper
                        .Map<IEnumerable<NotificationUserDetailDTO>>(notificationUsers
                        .OrderByDescending(nu => nu.IsNotificationViewed)
                        .ThenByDescending(nu => nu.Notification.NotificationCreatedAt)
                        .Skip(currentPage)
                        .Take(pageSize)), totalAmount);
            } catch {
                return (new List<NotificationUserDetailDTO>(), 0);
            }

        }

        public async Task<bool> SetViewedUserNotification(string username, int notificationId)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                ArgumentNullException.ThrowIfNull(user, nameof(username));
                var notificationUser = await _notificationUserRepository.GetByIdAsync((notificationId,user.UserId));
                notificationUser.IsNotificationViewed = true;
                await _notificationUserRepository.Update(notificationUser);
                return true;
            } catch(Exception ex) {
                return false;
            }
        }

    }
}

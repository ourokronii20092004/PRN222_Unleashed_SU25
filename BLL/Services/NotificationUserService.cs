using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;
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

        public Task DeleteUserNotification(string username, int notificationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NotificationUserDetailDTO>?> GetNotificationUserListAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                ArgumentNullException.ThrowIfNull(user, nameof(username));
                var NotificationUsers = await _notificationUserRepository.GetNotificationByUserId(user.UserId);
                return _mapper.Map<IEnumerable<NotificationUserDetailDTO>>(NotificationUsers);
            } catch {
                return null;
            }
        }

        public Task SetViewedUserNotification(string username, int notificationId)
        {
            throw new NotImplementedException();
        }
    }

}

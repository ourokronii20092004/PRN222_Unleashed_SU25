using DAL.DTOs.AccountDTOs;
using DAL.Models;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationDetailDTO
    {
        public int Id { get; set; }

        public Guid SenderId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public bool? IsDrafted { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }


        public static NotificationDetailDTO FromNotification(Notification notification)
        {
            return new NotificationDetailDTO
            {
                Id = notification.NotificationId,
                SenderId = notification.UserIdSender,
                Title = notification.NotificationTitle,
                Content = notification.NotificationContent,
                IsDrafted = notification.IsNotificationDraft,
                CreatedAt = notification.NotificationCreatedAt,
                UpdatedAt = notification.NotificationUpdatedAt
            };
        }

        public Notification toNotification()
        {
            return new Notification{
               NotificationId = Id,
               UserIdSender = SenderId,
               NotificationContent = Content,
               IsNotificationDraft = IsDrafted,
               NotificationCreatedAt = CreatedAt,
               NotificationUpdatedAt = UpdatedAt,
            };
        }
    }
}

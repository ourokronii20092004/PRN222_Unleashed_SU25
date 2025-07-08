using DAL.Models;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationDetailDTO
    {
        public int NotificationId { get; set; }

        public Guid UserIdSender { get; set; }

        public string? NotificationTitle { get; set; }

        public string? NotificationContent { get; set; }

        public bool IsNotificationDraft { get; set; }

        public DateTimeOffset? NotificationCreatedAt { get; set; }

        public DateTimeOffset? NotificationUpdatedAt { get; set; }

        public User? UserIdSenderNavigation { get; set; }
    }
}

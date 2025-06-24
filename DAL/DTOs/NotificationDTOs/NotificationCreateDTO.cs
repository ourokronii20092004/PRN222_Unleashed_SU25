using DAL.Models;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationCreateDTO
    {
        public Guid SenderId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public bool IsDrafted { get; set; } = false;

        public IEnumerable<string>? Receivers { get; set; }
        public Notification ToNotification()
        {
            return new Notification
            {
                UserIdSender = SenderId,
                NotificationTitle = Title,
                NotificationContent = Content,
                IsNotificationDraft = IsDrafted
            };

        }
    }
}

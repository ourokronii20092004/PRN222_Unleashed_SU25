using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationDetailDTO
    {
        public int NotificationId { get; set; }

        public Guid UserIdSender { get; set; }
        [Display(Name ="Title")]
        public string? NotificationTitle { get; set; }
        [Display(Name = "Content")]
        public string? NotificationContent { get; set; }

        public bool IsNotificationDraft { get; set; }
        [Display(Name = "Created At")]
        public DateTimeOffset? NotificationCreatedAt { get; set; }
        [Display(Name = "Updated At")]
        public DateTimeOffset? NotificationUpdatedAt { get; set; }
        [Display(Name = "Sender")]
        public User? UserIdSenderNavigation { get; set; }
    }
}

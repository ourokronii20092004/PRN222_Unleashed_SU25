using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationCreateDTO
    {
        public Guid UserIdSender { get; set; }

        [Display(Name ="Title")]
        public string? NotificationTitle { get; set; }
        [Display(Name = "Content")]
        public string? NotificationContent { get; set; }
        [Display(Name = "Draft")]
        public bool? IsNotificationDraft { get; set; } = false;

        public IEnumerable<string>? Receivers { get; set; }
       
    }
}

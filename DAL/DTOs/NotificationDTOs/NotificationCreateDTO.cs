using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationCreateDTO
    {
        public string? UsernameSender { get; set; }

        [Display(Name ="Title")]
        [Required(ErrorMessage = "Title can not be empty!")]
        public string? NotificationTitle { get; set; }
        [Display(Name = "Content")]
        [Required(ErrorMessage = "Content can not be empty!")]
        public string? NotificationContent { get; set; }
        [Display(Name = "Draft")]
        public bool IsNotificationDraft { get; set; } = false;
       
    }
}

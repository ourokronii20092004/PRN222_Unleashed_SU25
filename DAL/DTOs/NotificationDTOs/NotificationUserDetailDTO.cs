using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationUserDetailDTO
    {
        public int NotificationId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsNotificationViewed { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;
        [Display(Name ="Title")]
        public string? NotificationTitle { get; set; }
        [Display(Name = "Date")]
        public DateTimeOffset NotificationCreatedAt { get; set; }
    }
}

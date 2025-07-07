using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.NotificationDTOs
{
    public class NotificationUserDetailDTO
    {
        public int NotificationId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsViewed { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;
    }
}

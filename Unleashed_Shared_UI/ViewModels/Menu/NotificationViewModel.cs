using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.ViewModels.Menu
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsViewed { get; set; }
        public string TimeAgo { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class NotificationBellViewModel
    {
        public bool HasUnread { get; set; }
        public List<NotificationViewModel> Notifications { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class NotificationUser
{
    public int NotificationId { get; set; }

    public string UserId { get; set; } = null!;

    public bool? IsNotificationViewed { get; set; }

    public bool? IsNotificationDeleted { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class NotificationUser
{
    public int NotificationId { get; set; }

    public Guid UserId { get; set; }

    public bool? IsNotificationViewed { get; set; } = false;

    public bool? IsNotificationDeleted { get; set; } = false;

    public virtual Notification Notification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

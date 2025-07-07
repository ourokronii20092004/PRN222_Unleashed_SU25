using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unleashed_Shared_UI.ViewModels.Menu;

namespace Unleashed_Shared_UI.ViewComponents.Menu
{
    public class NotificationBellViewComponent : ViewComponent
    {
        // Iinject notification service here.
        // private readonly INotificationService _notificationService;
        // public NotificationBellViewComponent(INotificationService notificationService) { ... }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Content(string.Empty);
            }

            // This is where you would call your service to get notifications.
            // var notifications = await _notificationService.GetForUserAsync(User.Identity.Name);

            // For demonstration, we'll use mock data.
            var mockNotifications = new List<NotificationViewModel>
            {
                new() { Id = 1, Title = "New Order Received", Content = "Order #12345 has been confirmed.", IsViewed = false, CreatedAt = DateTime.UtcNow.AddMinutes(-5) },
                new() { Id = 2, Title = "Item Shipped", Content = "Your package is on its way.", IsViewed = true, CreatedAt = DateTime.UtcNow.AddHours(-2) },
            };

            // Map to ViewModel and calculate TimeAgo
            var notificationViewModels = mockNotifications.Select(n => new NotificationViewModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                IsViewed = n.IsViewed,
                TimeAgo = TimeAgo(n.CreatedAt) // Calculate relative time
            }).ToList();

            var model = new NotificationBellViewModel
            {
                Notifications = notificationViewModels,
                HasUnread = notificationViewModels.Any(n => !n.IsViewed)
            };

            return View(model);
        }

        // Helper function to calculate relative time
        private static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.UtcNow - dt;
            if (span.Days > 365) return $"{span.Days / 365} years ago";
            if (span.Days > 30) return $"{span.Days / 30} months ago";
            if (span.Days > 0) return $"{span.Days} days ago";
            if (span.Hours > 0) return $"{span.Hours} hours ago";
            if (span.Minutes > 0) return $"{span.Minutes} minutes ago";
            return "just now";
        }
    }
}
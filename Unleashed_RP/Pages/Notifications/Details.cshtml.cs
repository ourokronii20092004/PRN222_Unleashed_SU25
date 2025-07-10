using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;
using Microsoft.Extensions.Logging;

namespace Unleashed_RP.Pages.Notifications
{
    [Filter.Filter(RequiredRoles = new[]{"CUSTOMER"})]
    public class DetailsModel : PageModel
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationUserService _notificationUserService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(INotificationService notificationService, INotificationUserService notificationUserService, ILogger<DetailsModel> logger)
        {
            _notificationService = notificationService;
            _notificationUserService = notificationUserService;
            _logger = logger;
        }

        public NotificationDetailDTO NotificationDetailDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var notification = await _notificationService.GetNotificationByIdAsync(id.Value);

                if (notification == null)
                {
                    return NotFound();
                }
                else
                {
                    string? username = HttpContext.Session.GetString("username");
                    ArgumentNullException.ThrowIfNullOrEmpty(username);
                    await _notificationUserService.SetViewedUserNotification(username, id.Value);
                    NotificationDetailDTO = notification;
                }
                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("../Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int NotificationId) {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);

            await _notificationUserService.DeleteUserNotification(username, NotificationId);
            return RedirectToPage("./Index");
        }
    }
}

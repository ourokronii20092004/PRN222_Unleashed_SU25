using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;

namespace Unleashed_RP.Pages.Notifications
{
    public class IndexModel : PageModel
    {
        private readonly INotificationUserService _notificationUserService;

        public IndexModel(INotificationUserService notificationUserService)
        {
            _notificationUserService = notificationUserService;
        }

        public IList<NotificationUserDetailDTO> NotificationUsers { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                string? username = HttpContext.Session.GetString("username");
                ArgumentNullException.ThrowIfNullOrEmpty(username, nameof(username));
                NotificationUsers = [.. await _notificationUserService.GetNotificationUserListAsync(username)];
            } catch (ArgumentNullException ex) {
                RedirectToPage("../Index");
            }
        }
    }
}

using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unleashed_RP.Pages.Notifications
{
    [Filter.Filter(RequiredRoles = new[] { "CUSTOMER" })]
    public class IndexModel : PageModel
    {
        private readonly INotificationUserService _notificationUserService;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageIndex { get; set; }

        public int pageSize = 5;
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
                int currentPage = pageIndex ?? 1;
                var (NotificationUsersList, totalAmount) = await _notificationUserService.GetNotificationUserListAsync(username, SearchString, currentPage, pageSize);
                ViewData["Pages"] = (totalAmount + pageSize - 1) / pageSize;
                ViewData["CurrentPage"] = currentPage;
                ViewData["SearchString"] = SearchString;
                NotificationUsers = (IList<NotificationUserDetailDTO>)NotificationUsersList;

            } catch (ArgumentNullException ex) {
                RedirectToPage("../Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int NotificationId)
        {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);

            await _notificationUserService.DeleteUserNotification(username, NotificationId);
            return RedirectToPage("./Index");
        }
    }
}

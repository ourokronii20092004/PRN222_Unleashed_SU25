using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;

namespace Unleashed_RP.Pages.Notifications
{
    public class DetailsModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public DetailsModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public NotificationDetailDTO NotificationDetailDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
                NotificationDetailDTO = notification;
            }
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.DTOs.NotificationDTOs;
using BLL.Services.Interfaces;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _accountService;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageIndex { get; set; }

        public int pageSize = 10;
        public NotificationsController(INotificationService notificationService, IUserService accountService)
        {
            _notificationService = notificationService;
            _accountService = accountService;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            int currentPage = pageIndex ?? 1;
            var (notiList, totalAmount) = await _notificationService.GetAllNotificationsAsync(SearchString, currentPage, pageSize);
            ViewData["Pages"] = (totalAmount + pageSize - 1) / pageSize;
            ViewData["CurrentPage"] = currentPage;
            ViewData["SearchString"] = SearchString;
            return View(notiList);
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NotificationTitle,NotificationContent,IsNotificationDraft")] NotificationCreateDTO notification)
        {
            notification.UsernameSender = HttpContext.Session.GetString("username");
            if (await _notificationService.AddNotificationAsync(notification))
            {            
                return RedirectToAction(nameof(Index));
            }          
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _notificationService.GetNotificationByIdAsync(id.Value);
            if (notification == null || !notification.IsNotificationDraft)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NotificationDetailDTO notificationDTO)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(notificationDTO.NotificationId);
            if ( notification != null)
            {
                try
                {

                    if (!await _notificationService.EditNotificationAsync(notificationDTO))
                    {
                       return View(notification);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            } 
            return NotFound();
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int NotificationId)
        {
            
              await _notificationService.RemoveNotificationAsync(NotificationId);
            

            return RedirectToAction(nameof(Index));
        }
    }
}

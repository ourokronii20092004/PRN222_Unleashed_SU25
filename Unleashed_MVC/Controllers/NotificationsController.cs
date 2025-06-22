using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using BLL.Interfaces;

namespace Unleashed_MVC.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;

        public NotificationsController(INotificationService notificationService, IAccountService accountService)
        {
            _notificationService = notificationService;
            _accountService = accountService;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
           
            return View(await _notificationService.GetAllNotificationsAsync());
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
        public async Task<IActionResult> Create()
        {
            ViewData["Sender"] = new SelectList(await _accountService.GetAccountsAsync(), "Username", "Username");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NotificationId,UserIdSender,NotificationTitle,NotificationContent,IsNotificationDraft,NotificationCreatedAt,NotificationUpdatedAt")] Notification notification, IEnumerable<string> usernames)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<User> users = new List<User>();
                await _notificationService.AddNotificationAsync(notification, users);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Sender"] = new SelectList(await _accountService.GetAccountsAsync(), "Username", "Username");
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
            if (notification == null)
            {
                return NotFound();
            }
            ViewData["Sender"] = new SelectList(await _accountService.GetAccountsAsync(), "Username", "Username");
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NotificationId,UserIdSender,NotificationTitle,NotificationContent,IsNotificationDraft,NotificationCreatedAt,NotificationUpdatedAt")] Notification notification)
        {
            if (id != notification.NotificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<User> users = new List<User>();
                    await _notificationService.EditNotificationAsync(notification, users);
                }
                catch (DbUpdateConcurrencyException)
                {                  
                        return NotFound();                               
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Sender"] = new SelectList(await _accountService.GetAccountsAsync(), "Username", "Username");
            return View(notification);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
              await _notificationService.RemoveNotificationAsync(id);
            

            return RedirectToAction(nameof(Index));
        }
    }
}

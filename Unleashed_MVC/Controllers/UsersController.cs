using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using BLL.Interfaces;

namespace Unleashed_MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAccountService _service;

        public UsersController(IAccountService service)
        {
            _service = service;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var accountList = await _service.GetAccountsAsync();
            return View(accountList);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {         
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId,IsUserEnabled,UserGoogleId,UserUsername,UserPassword,UserFullname,UserEmail,UserPhone,UserBirthdate,UserAddress,UserImage,UserCurrentPaymentMethod,UserCreatedAt,UserUpdatedAt,Gender")] User user)
        {
            if (ModelState.IsValid)
            {

                await _service.AddUser(user);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, [Bind("UserId,RoleId,IsUserEnabled,UserGoogleId,UserUsername,UserPassword,UserFullname,UserEmail,UserPhone,UserBirthdate,UserAddress,UserImage,UserCurrentPaymentMethod,UserCreatedAt,UserUpdatedAt,Gender")] User user)
        {
            if (username != user.UserUsername)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {                
                if(await _service.EditUser(user)) 
                    return NotFound();      
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username)
        {
            var user = await _service.GetUserByUsername(username);
            if (user != null)
            {
               await _service.DeleteUser(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

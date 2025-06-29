using Microsoft.AspNetCore.Mvc;
using DAL.DTOs.UserDTOs;
using BLL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter]
    public class UsersController : Controller
    {
        private readonly IUserService _service;


        public UsersController(IUserService service)
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

            var user = await _service.GetUserByUsernameAsync(username);
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
        public async Task<IActionResult> Create(RegisterUserDTO user, IFormFile profileImageFile)
        {
            if(_service.GetUserByUsernameAsync(user.UserUsername).Result != null)
            {
                ModelState.AddModelError(string.Empty, "Username has been existed, Please use a new username.");
                return View(user);
            }
            if (await _service.AddUserAsync(user, 2, profileImageFile))
            {            
                return RedirectToAction(nameof(Index));
            }
            else
            {  
                ModelState.AddModelError(string.Empty, "Could not create account. Please check the information provided.");
                return View(user);
            }

        }

        public async Task<IActionResult> Edit(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDetailDTO user, IFormFile profileImageFile)
        {
            try {
                ArgumentNullException.ThrowIfNull(await _service.GetUserByUsernameAsync(user.UserUsername));
                if (!await _service.EditUserAsync(user, profileImageFile))
                    return View(user);

                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                return NotFound(); 
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByUsernameAsync(username);
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
            if (!username.IsNullOrEmpty())
            {
               if (!await _service.DeleteUserAsync(username))
                   NotFound() ;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

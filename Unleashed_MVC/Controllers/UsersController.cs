using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL.DTOs.AccountDTOs;
using BLL.Services.Interfaces;

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

            var user = await _service.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(AccountDetailDTO.FromUser(user));
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
        public async Task<IActionResult> Create([Bind("Username,Password,Fullname,Email,Phone,Birthdate,Address,Image,Gender")] RegisterAccountDTO account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }     
            
            var user = account.ToUser(); 

            if (await _service.AddUserAsync(user, 2))
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
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user.RoleId);
            return View(AccountDetailDTO.FromUser(user));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit ([Bind("Username,Fullname,Email,Phone,BirthDate,Address,Image,Gender,IsEnabled")] AccountDetailDTO account)
        {
   
            if (ModelState.IsValid)
            {
                if (!await _service.EditUserAsync(account.ToUser())) 
                    return NotFound();      
                
                return RedirectToAction(nameof(Index));
            }
 
            return View(account);
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
            var user = await _service.GetUserByUsernameAsync(username);
            if (user != null)
            {
               await _service.DeleteUserAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

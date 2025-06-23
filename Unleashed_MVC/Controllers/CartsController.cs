using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        //private Guid GetCurrentUserId()
        //{
        //    {
        //        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        //        return claim != null ? Guid.Parse(claim.Value) : throw new Exception("Not authenticated");
        //    }
        //}
        public CartsController(ICartService cartService, IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }
       
        // GET: Carts
        public async Task<IActionResult> Index()
        {
            
            return View(await _cartService.GetAllAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(Guid userId, int variationId)
        {
            var cart = await _cartService.GetCartByIdAsync((userId, variationId));
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            //var variations = await _cartService.GetAllVariationsAsync(); // thêm method này vào interface nếu chưa có
            //ViewData["VariationId"] = new SelectList(variations, "VariationId", "VariationId");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,VariationId,CartQuantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                await _cartService.CreateCartAsync(cart);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList( await _accountService.GetAccountsAsync(), "Username", "Username");
            //var variations = await _cartService.GetAllVariationsAsync();
            //ViewData["VariationId"] = new SelectList(variations, "VariationId", "VariationId");
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(Guid userId, int variationId)
        {

            var cart = await _cartService.GetCartByIdAsync((userId, variationId));
            if (cart == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", cart.UserId);
            //ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId", cart.VariationId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,VariationId,CartQuantity")] Cart cart)
        {
            if (id != cart.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _cartService.UpdateCartAsync(cart);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", cart.UserId);
            //ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId", cart.VariationId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(Guid userId, int variationId)
        {

            var cart = await _cartService.GetCartByIdAsync((userId, variationId));
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed((Guid userId, int variationId) id)
        {
            await _cartService.DeleteCartAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

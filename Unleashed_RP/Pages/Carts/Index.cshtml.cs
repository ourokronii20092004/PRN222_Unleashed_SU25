using BLL.Services;
using BLL.Services.Interfaces;
using DAL.DTOs.CartDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unleashed_RP.Pages.Carts
{
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public IndexModel(ICartService cartService, IUserService userService, IOrderService orderService)
        {
            _cartService = cartService;
            _userService = userService;
            _orderService = orderService;
        }
        public Dictionary<string, List<CartDTO>> GroupedCartItems { get; set; } = new();
        public IList<Cart> Cart { get; set; } = default!;

        public async Task OnGetAsync()
        {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            GroupedCartItems = await _cartService.GetCartByUsernameAsync(username);
        }

        public async Task<IActionResult> OnPostRemoveAsync(int variationId)
        {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            if (!string.IsNullOrEmpty(username))
            {
                await _cartService.RemoveFromCartAsync(username, variationId);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            if (!string.IsNullOrEmpty(username))
            {
                await _cartService.RemoveAllFromCartAsync(username);
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUpdateQuantityAsync(int variationId, int quantity)
        {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            if (!string.IsNullOrEmpty(username) && quantity > 0)
            {
                await _cartService.UpdateQuantityAsync(username, variationId, quantity);
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            try
            {
                string? username = HttpContext.Session.GetString("username");
                ArgumentNullException.ThrowIfNullOrEmpty(username);
                var orderId = await _orderService.ConvertCartToOrderAsync(username);
                return RedirectToPage("/Orders/Index", new { id = orderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }
}

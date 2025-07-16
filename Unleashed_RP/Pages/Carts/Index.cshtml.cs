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
        private readonly IVnpayService _vnpayService;
        public IndexModel(ICartService cartService, IUserService userService, IOrderService orderService, IVnpayService vnpayService)
        {
            _cartService = cartService;
            _userService = userService;
            _orderService = orderService;
            _vnpayService = vnpayService;
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

                // Get user info
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Convert cart to order and get order details
                var (orderId, totalAmount) = await _orderService.ConvertCartToOrderAsync(username);

                // Create payment info
                var paymentModel = new PaymentInfoModel
                {
                    OrderId = orderId,
                    Amount = totalAmount,
                    Name = user.UserFullname ?? "Customer",
                    OrderDescription = $"Payment for order #{orderId}"
                };

                // Create payment URL and redirect
                var paymentUrl = _vnpayService.CreatePaymentUrl(paymentModel, HttpContext);
                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await OnGetAsync(); // Reload cart items
                return Page();
            }
        }

        }
}

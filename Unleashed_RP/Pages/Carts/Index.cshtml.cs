using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.CartDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Carts
{
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;

        public IndexModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IList<Cart> Cart { get;set; } = default!;
        public List<CartDTO> CartItems { get; set; } = new List<CartDTO>();
        public async Task OnGetAsync()
        {
            // Lấy UserName từ HttpContext
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                // Có thể redirect hoặc xử lý khác nếu chưa đăng nhập
                CartItems = new List<CartDTO>();
                return;
            }

            try
            {
                // Gọi service để lấy UserId từ username
                var userId = await _cartService.GetUserIdByUserNameAsync(userName);

                if (userId == null)
                {
                    CartItems = new List<CartDTO>();
                    return;
                }

                // Gọi service lấy cart items theo UserId
                CartItems = await _cartService.GetCartItemsAsync(userId.Value);
            }
            catch (Exception ex)
            {
                // Log lỗi và tránh crash trang
                CartItems = new List<CartDTO>();
                // Optionally: ModelState.AddModelError("", ex.Message);
            }
        }
    }
}

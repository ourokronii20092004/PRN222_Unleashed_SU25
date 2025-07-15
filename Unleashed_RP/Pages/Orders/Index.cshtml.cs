using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.OderDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public List<OrderDTO> Orders { get; set; }

        public IndexModel(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        public async Task OnGetAsync()
        {
            string? username = HttpContext.Session.GetString("username");
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            var userId = await _cartService.GetUserIdByUsername(username);
            Orders = (await _orderService.GetOrdersByUserAsync(userId)).ToList();
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(Guid orderId)
        {
            var order = await _orderService.GetOrderDetailAsync(orderId);
            if (order?.OrderStatusId == 5) // Only allow cancel for Pending orders
            {
                await _orderService.CancelOrderAsync(orderId);
            }
            return RedirectToPage();
        }
        
    }
}

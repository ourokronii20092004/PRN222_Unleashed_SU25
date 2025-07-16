using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Unleashed_RP.Pages.Payment
{
    [Filter.Filter(RequiredRoles = new[] { "CUSTOMER" })]
    public class LandingModel : PageModel
    {
        private readonly IVnpayService _vnpayService;
        private readonly IOrderService _orderService;

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public LandingModel(IVnpayService vnpayService, IOrderService orderService)
        {
            _vnpayService = vnpayService;
            _orderService = orderService;
        }

        public async Task<IActionResult> OnGet()
        {
            var response = _vnpayService.GetPaymentResponse(Request.Query);

            if (response == null)
            {
                Message = "There was an error processing your payment. Invalid response.";
                IsSuccess = false;
                return Page();
            }

            var orderId = response.OrderId.Value;
            var order = await _orderService.GetOrderDetailAsync(orderId);

            if (order == null)
            {
                Message = $"Error: Payment was processed but the corresponding order ({orderId}) was not found.";
                IsSuccess = false;
                return Page();
            }

            if (response.Success)
            {
                await _orderService.ApproveOrderAsync(orderId);
                Message = $"Thank you! Your payment for order #{order.OrderId} was successful.";
                IsSuccess = true;
            }
            else
            {
                await _orderService.CancelOrderAsync(orderId);
                Message = $"Payment for order #{order.OrderId} failed. Please try again.";
                IsSuccess = false;
            }

            return Page();




        }
    }
}

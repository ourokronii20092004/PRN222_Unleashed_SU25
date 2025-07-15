using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Data;
using DAL.Models;
using BLL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Unleashed_RP.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;
        private readonly IVnpayService _vnpayService;

        private const int PENDING_STATUS_ID = 5;

        public CreateModel(DAL.Data.UnleashedContext context, IVnpayService vnpayService)
        {
            _context = context;
            _vnpayService = vnpayService;
        }

        public IActionResult OnGet()
        {
        ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode");
        ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
        ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId");
        ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.OrderStatusId = PENDING_STATUS_ID;
            Order.OrderDate = DateTime.Now;

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            var paymentModel = new PaymentInfoModel
            {
                OrderId = Order.OrderId,
                Amount = Order.OrderTotalAmount,
                Name = Order.User.UserFullname,
                OrderDescription = $"Payment for order #{Order.OrderId}"
            };

            return RedirectToPage(_vnpayService.CreatePaymentUrl(paymentModel, HttpContext));
        }
    }
}

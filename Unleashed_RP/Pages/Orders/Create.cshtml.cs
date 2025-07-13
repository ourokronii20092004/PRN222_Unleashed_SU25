using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Data;
using DAL.Models;

namespace Unleashed_RP.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;

        public CreateModel(DAL.Data.UnleashedContext context)
        {
            _context = context;
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

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

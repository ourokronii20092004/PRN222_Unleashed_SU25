using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;

namespace Unleashed_RP.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;

        public EditModel(DAL.Data.UnleashedContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order =  await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
           ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode");
           ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId");
           ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
           ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId");
           ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId");
           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}

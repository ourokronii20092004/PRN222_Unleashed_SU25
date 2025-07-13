using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;

namespace Unleashed_RP.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;

        public IndexModel(DAL.Data.UnleashedContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.Discount)
                .Include(o => o.InchargeEmployee)
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ShippingMethod)
                .Include(o => o.User).ToListAsync();
        }
    }
}

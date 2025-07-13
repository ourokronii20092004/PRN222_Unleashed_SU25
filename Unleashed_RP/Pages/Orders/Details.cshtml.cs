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
    public class DetailsModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;

        public DetailsModel(DAL.Data.UnleashedContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }
    }
}

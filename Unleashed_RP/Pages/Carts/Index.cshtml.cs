using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;

namespace Unleashed_RP.Pages.Carts
{
    public class IndexModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;

        public IndexModel(DAL.Data.UnleashedContext context)
        {
            _context = context;
        }

        public IList<Cart> Cart { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Cart = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.Variation).ToListAsync();
        }
    }
}

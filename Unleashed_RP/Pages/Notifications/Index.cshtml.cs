using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;

namespace Unleashed_RP.Pages.Notifications
{
    public class IndexModel : PageModel
    {
        private readonly DAL.Data.UnleashedContext _context;

        public IndexModel(DAL.Data.UnleashedContext context)
        {
            _context = context;
        }

        public IList<NotificationUser> NotificationUser { get;set; } = default!;

        public async Task OnGetAsync()
        {
            NotificationUser = await _context.NotificationUsers
                .Include(n => n.Notification)
                .Include(n => n.User).ToListAsync();
        }
    }
}

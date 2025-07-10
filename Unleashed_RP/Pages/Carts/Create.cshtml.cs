using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Data;
using DAL.Models;

namespace Unleashed_RP.Pages.Carts
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
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId");
            return Page();
        }

        [BindProperty]
        public Cart Cart { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Carts.Add(Cart);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

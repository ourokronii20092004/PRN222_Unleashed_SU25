using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.OderDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderDTO Order { get; set; } = default!;

        public DetailsModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _orderService.GetOrderDetailAsync(id.Value);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? statusId)
        {
            var orderDtos = await _orderService.GetOrdersAsync();

            if (statusId.HasValue)
            {
                orderDtos = orderDtos.Where(o => o.OrderStatusId == statusId.Value);
            }

            // Map DTOs to Models
            var orderModels = _mapper.Map<IEnumerable<Order>>(orderDtos);

            return View(orderModels);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var orderDto = await _orderService.GetOrderDetailAsync(id);
            if (orderDto == null) return NotFound();

            // Map DTO to Model
            var orderModel = _mapper.Map<Order>(orderDto);
            return View(orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _orderService.ApproveOrderAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}

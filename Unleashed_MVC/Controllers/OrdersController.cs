using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using BLL.Services;
using BLL.Services.Interfaces;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _orderService.GetAllOrdersAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            //ViewData["DiscountId"] = new SelectList(_orderService.Discounts, "DiscountId", "DiscountCode");
            //ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId");
            //ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            //ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId");
            //ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId");
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,OrderStatusId,PaymentMethodId,ShippingMethodId,DiscountId,InchargeEmployeeId,OrderDate,OrderTotalAmount,OrderTrackingNumber,OrderNote,OrderBillingAddress,OrderExpectedDeliveryDate,OrderTransactionReference,OrderTax,OrderCreatedAt,OrderUpdatedAt")] Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrder(order);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode", order.DiscountId);
            //ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", order.InchargeEmployeeId);
            //ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", order.OrderStatusId);
            //ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId", order.PaymentMethodId);
            //ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId", order.ShippingMethodId);
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode", order.DiscountId);
            //ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", order.InchargeEmployeeId);
            //ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", order.OrderStatusId);
            //ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId", order.PaymentMethodId);
            //ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId", order.ShippingMethodId);
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OrderId,UserId,OrderStatusId,PaymentMethodId,ShippingMethodId,DiscountId,InchargeEmployeeId,OrderDate,OrderTotalAmount,OrderTrackingNumber,OrderNote,OrderBillingAddress,OrderExpectedDeliveryDate,OrderTransactionReference,OrderTax,OrderCreatedAt,OrderUpdatedAt")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateOrderAsync(id, order);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode", order.DiscountId);
            //ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", order.InchargeEmployeeId);
            //ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", order.OrderStatusId);
            //ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId", order.PaymentMethodId);
            //ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId", order.ShippingMethodId);
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;

namespace Unleashed_MVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UnleashedContext _context;

        public OrdersController(UnleashedContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var unleashedContext = _context.Orders.Include(o => o.Discount).Include(o => o.InchargeEmployee).Include(o => o.OrderStatus).Include(o => o.PaymentMethod).Include(o => o.ShippingMethod).Include(o => o.User);
            return View(await unleashedContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Discount)
                .Include(o => o.InchargeEmployee)
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ShippingMethod)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode");
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId");
            ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
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
                order.OrderId = Guid.NewGuid();
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode", order.DiscountId);
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", order.InchargeEmployeeId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", order.OrderStatusId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId", order.PaymentMethodId);
            ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId", order.ShippingMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode", order.DiscountId);
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", order.InchargeEmployeeId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", order.OrderStatusId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId", order.PaymentMethodId);
            ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId", order.ShippingMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
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
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountCode", order.DiscountId);
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", order.InchargeEmployeeId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", order.OrderStatusId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "PaymentMethodId", "PaymentMethodId", order.PaymentMethodId);
            ViewData["ShippingMethodId"] = new SelectList(_context.ShippingMethods, "ShippingMethodId", "ShippingMethodId", order.ShippingMethodId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Discount)
                .Include(o => o.InchargeEmployee)
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ShippingMethod)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
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
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}

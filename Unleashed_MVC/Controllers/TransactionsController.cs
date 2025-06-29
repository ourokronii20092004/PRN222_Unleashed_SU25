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
    [Filter.Filter]
    public class TransactionsController : Controller
    {
        private readonly UnleashedContext _context;

        public TransactionsController(UnleashedContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var unleashedContext = _context.Transactions.Include(t => t.InchargeEmployee).Include(t => t.Provider).Include(t => t.Stock).Include(t => t.TransactionType).Include(t => t.Variation);
            return View(await unleashedContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.InchargeEmployee)
                .Include(t => t.Provider)
                .Include(t => t.Stock)
                .Include(t => t.TransactionType)
                .Include(t => t.Variation)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId");
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockId");
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "TransactionTypeId");
            ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,StockId,VariationId,ProviderId,InchargeEmployeeId,TransactionTypeId,TransactionQuantity,TransactionDate,TransactionProductPrice")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", transaction.InchargeEmployeeId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", transaction.ProviderId);
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockId", transaction.StockId);
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "TransactionTypeId", transaction.TransactionTypeId);
            ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId", transaction.VariationId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", transaction.InchargeEmployeeId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", transaction.ProviderId);
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockId", transaction.StockId);
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "TransactionTypeId", transaction.TransactionTypeId);
            ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId", transaction.VariationId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,StockId,VariationId,ProviderId,InchargeEmployeeId,TransactionTypeId,TransactionQuantity,TransactionDate,TransactionProductPrice")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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
            ViewData["InchargeEmployeeId"] = new SelectList(_context.Users, "UserId", "UserId", transaction.InchargeEmployeeId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", transaction.ProviderId);
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockId", transaction.StockId);
            ViewData["TransactionTypeId"] = new SelectList(_context.TransactionTypes, "TransactionTypeId", "TransactionTypeId", transaction.TransactionTypeId);
            ViewData["VariationId"] = new SelectList(_context.Variations, "VariationId", "VariationId", transaction.VariationId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.InchargeEmployee)
                .Include(t => t.Provider)
                .Include(t => t.Stock)
                .Include(t => t.TransactionType)
                .Include(t => t.Variation)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}

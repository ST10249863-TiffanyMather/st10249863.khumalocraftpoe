using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft_Part2.Data;
using KhumaloCraft_Part2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KhumaloCraft_Part2.Controllers
{
    [Authorize]
    public class ProductTransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTransaction
        public async Task<IActionResult> Index(string status)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            IQueryable<ProductTransaction> transactions = _context.ProductTransaction
                .Include(pt => pt.Transaction)
                    .ThenInclude(t => t.User)
                .Include(pt => pt.Product)
                .Where(pt => pt.Transaction.UserId == user.UserId);

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                transactions = transactions.Where(pt => pt.Transaction.TransactionStatus == status);
            }

            var transactionList = await transactions.ToListAsync();

            ViewBag.SelectedStatus = status;

            return View(transactionList);
        }


        // GET: ProductTransaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTransaction = await _context.ProductTransaction
                .Include(p => p.Product)
                .Include(p => p.Transaction)
                .FirstOrDefaultAsync(m => m.ProductTransactionId == id);
            if (productTransaction == null)
            {
                return NotFound();
            }

            return View(productTransaction);
        }

        // GET: ProductTransaction/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            ViewData["TransactionID"] = new SelectList(_context.Transaction, "TransactionId", "TransactionId");
            return View();
        }

        // POST: ProductTransaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTransactionId,TransactionID,ProductId")] ProductTransaction productTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", productTransaction.ProductId);
            ViewData["TransactionID"] = new SelectList(_context.Transaction, "TransactionId", "TransactionId", productTransaction.TransactionID);
            return View(productTransaction);
        }

        // GET: ProductTransaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTransaction = await _context.ProductTransaction.FindAsync(id);
            if (productTransaction == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", productTransaction.ProductId);
            ViewData["TransactionID"] = new SelectList(_context.Transaction, "TransactionId", "TransactionId", productTransaction.TransactionID);
            return View(productTransaction);
        }

        // POST: ProductTransaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTransactionId,TransactionID,ProductId")] ProductTransaction productTransaction)
        {
            if (id != productTransaction.ProductTransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTransactionExists(productTransaction.ProductTransactionId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", productTransaction.ProductId);
            ViewData["TransactionID"] = new SelectList(_context.Transaction, "TransactionId", "TransactionId", productTransaction.TransactionID);
            return View(productTransaction);
        }

        // GET: ProductTransaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTransaction = await _context.ProductTransaction
                .Include(p => p.Product)
                .Include(p => p.Transaction)
                .FirstOrDefaultAsync(m => m.ProductTransactionId == id);
            if (productTransaction == null)
            {
                return NotFound();
            }

            return View(productTransaction);
        }

        // POST: ProductTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTransaction = await _context.ProductTransaction.FindAsync(id);
            if (productTransaction != null)
            {
                _context.ProductTransaction.Remove(productTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTransactionExists(int id)
        {
            return _context.ProductTransaction.Any(e => e.ProductTransactionId == id);
        }
    }
}

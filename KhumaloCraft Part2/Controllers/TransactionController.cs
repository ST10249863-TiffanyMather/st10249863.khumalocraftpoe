using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft_Part2.Data;
using KhumaloCraft_Part2.Models;

namespace KhumaloCraft_Part2.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        //****************
        //Code Attribution to filter was taken from StackOverflow:
        //Author: Aadil Khatri
        //Link: https://stackoverflow.com/questions/51770087/asp-net-mvc-c-sharp-web-app-filtering-search-from-db-getting-error
        //****************

        // GET: Transaction
        public async Task<IActionResult> Index(string status)
        {
            IQueryable<ProductTransaction> transactions = _context.ProductTransaction
                .Include(pt => pt.Transaction)
                    .ThenInclude(t => t.User)
                .Include(pt => pt.Product);

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                transactions = transactions.Where(pt => pt.Transaction.TransactionStatus == status);
            }

            var transactionList = await transactions.ToListAsync();

            ViewBag.SelectedStatus = status;

            return View(transactionList);
        }




        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,UserId,TransactionDate,TransactionTotalPrice,TransactionPaymentMethod,TransactionStatus")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", transaction.UserId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.User) 
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            var productNames = await _context.ProductTransaction
                .Where(pt => pt.TransactionID == id)
                .Include(pt => pt.Product)
                .Select(pt => pt.Product.ProductName)
                .ToListAsync();


            ViewData["UserEmail"] = transaction.User.UserEmail;
            ViewData["ProductNames"] = productNames;

            return View(transaction);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,UserId,TransactionDate,TransactionTotalPrice,TransactionPaymentMethod,TransactionStatus")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTransaction = await _context.Transaction.FindAsync(id);

                    existingTransaction.TransactionDate = transaction.TransactionDate;
                    existingTransaction.TransactionTotalPrice = transaction.TransactionTotalPrice;
                    existingTransaction.TransactionPaymentMethod = transaction.TransactionPaymentMethod;
                    existingTransaction.TransactionStatus = transaction.TransactionStatus;

                    _context.Update(existingTransaction);

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

            ViewData["UserEmail"] = _context.User.FirstOrDefault(u => u.UserId == transaction.UserId)?.UserEmail;

            var productNames = await _context.ProductTransaction
                .Where(pt => pt.TransactionID == id)
                .Include(pt => pt.Product)
                .Select(pt => pt.Product.ProductName)
                .ToListAsync();

            ViewData["ProductNames"] = productNames;

            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}

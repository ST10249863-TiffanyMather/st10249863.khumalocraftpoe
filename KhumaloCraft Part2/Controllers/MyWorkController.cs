using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using KhumaloCraft_Part2.Data;
using KhumaloCraft_Part2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KhumaloCraft_Part2.Controllers
{
    [Authorize]
    public class MyWorkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyWorkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyWork
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from p in _context.Product select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }

            return View(await products.ToListAsync());
        }


        // GET: MyWork/PlaceOrder
        public async Task<IActionResult> PlaceOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: MyWork/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(int id, string streetAddress, string city, string country, string paymentMethod, string cardNumber, string cvv, string expiryDate)
        {
            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            product.ProductAvailable = false;
            _context.Update(product);
            await _context.SaveChangesAsync();


            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
            if (user == null)
            {
                return NotFound("User not found."); ;
            }

            var transaction = new Transaction
            {
                UserId = user.UserId,
                TransactionDate = DateTime.Now,
                TransactionTotalPrice = product.ProductPrice * 1.1, // 10% shipping fee
                TransactionPaymentMethod = paymentMethod,
                TransactionStatus = "Pending"
            };

            _context.Add(transaction);
            await _context.SaveChangesAsync();

            var productTransaction = new ProductTransaction
            {
                TransactionID = transaction.TransactionId,
                ProductId = product.ProductId
            };

            _context.Add(productTransaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(OrderConfirmation));
        }

        public IActionResult OrderConfirmation()
        {
            return View(); 
        }
    }
}

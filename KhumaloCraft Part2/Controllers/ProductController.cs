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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductCategory,ProductAvailable,ProductImage,ImageFile,ProductArtisan")] Product product)
        {
            //****************
            //Code Attribution
            //The following code to store images was taken from StackOverflow:
            //Author: Luca Geretti
            //Link: https://stackoverflow.com/questions/1064786/where-do-you-store-images-for-asp-net-mvc-projects-and-how-do-you-reference-them
            //****************

            if (product.ImageFile != null)
            {
                using (Stream fs = product.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        product.ProductImage = Convert.ToBase64String(bytes);
                    }
                }
            }

            if (product.ProductPrice.HasValue)
            {
                var price = product.ProductPrice.Value;
                if (price < 0.01 || price > 9999999.99 || Math.Round(price, 2) != price)
                {
                    ModelState.AddModelError("ProductPrice", "Price must be between 0.01 and 9999999.99 and have up to two decimal places.");
                }
            }

            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                ModelState.AddModelError("ProductName", "Product name is required.");
            }

            if (string.IsNullOrWhiteSpace(product.ProductDescription))
            {
                ModelState.AddModelError("ProductDescription", "Product description is required.");
            }

            if (string.IsNullOrWhiteSpace(product.ProductArtisan))
            {
                ModelState.AddModelError("ProductArtisan", "Artisan name is required.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductCategory,ProductAvailable,ProductImage,ImageFile,ProductArtisan")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (product.ImageFile != null)
            {
                using (Stream fs = product.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        product.ProductImage = Convert.ToBase64String(bytes);
                    }
                }
            }
            else
            {
                var currentProduct = await _context.Product
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.ProductId == id);
                if (currentProduct != null)
                {
                    product.ProductImage = currentProduct.ProductImage;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }


        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}

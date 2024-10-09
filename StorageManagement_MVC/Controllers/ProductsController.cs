using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StorageManagement.Models;
using StorageManagement_MVC.Data;
using static System.Reflection.Metadata.BlobBuilder;

namespace StorageManagement_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StorageManagement_MVCContext _context;

        public ProductsController(StorageManagement_MVCContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index(string SearchString)
        {
            var ListProducts = from p in _context.Product select p;
            if (!String.IsNullOrEmpty(SearchString))
            {
                ListProducts = ListProducts.Where(s =>
                    s.Id!.Contains(SearchString) ||
                    s.Name!.Contains(SearchString));
            }
            return View(await ListProducts.ToListAsync());
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ProductInventory(string SearchString)
        {
            var ListProductsInventory = from p in _context.productInventory select p;
            if (!String.IsNullOrEmpty(SearchString))
            {
                ListProductsInventory = ListProductsInventory.Where(s =>
                    s.Id!.Contains(SearchString) ||
                    s.Name!.Contains(SearchString));
            }
            return View(await ListProductsInventory.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        private string GenerateNewProductId()
        {
            var lastProduct = _context.Product.OrderByDescending(p => p.Id).FirstOrDefault();
            if (lastProduct != null && lastProduct.Id.StartsWith("SP"))
            {
                int lastIdNumber = int.Parse(lastProduct.Id.Substring(2));
                return "SP" + (lastIdNumber + 1).ToString("D4");
            }
            else
            {
                return "SP0001";
            }
        }


        // GET: Products/Create
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,priceInput,priceOutput")] Product product)
        {
            if (ModelState.IsValid)
            {
                productInventory pi = new productInventory();
                product.Id = GenerateNewProductId();
                product.priceOutput = product.priceInput + product.priceInput * 25 / 100;
                pi.Id = product.Id;
                pi.Name = product.Name;
                pi.Category = product.Category;
                pi.priceInput = product.priceInput;
                pi.Quantity = 0;
                pi.priceOutput = pi.priceInput + pi.priceInput * 25 / 100;
                pi.totalPrice = 0;
                _context.Add(product);
                _context.Add(pi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(string id)
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Category,priceInput,priceOutput")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Product.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }
                    existingProduct.priceInput = product.priceInput;
                    existingProduct.priceOutput = product.priceInput * 125 / 100;
                    existingProduct.Name = product.Name;
                    existingProduct.Category = product.Category;

                    _context.Update(existingProduct);

                    var existingInventory = await _context.productInventory.FindAsync(id);
                    if (existingInventory == null)
                    {
                        existingInventory = new productInventory();
                        existingInventory.Id = product.Id;
                        existingInventory.priceInput = product.priceInput;
                        existingInventory.priceOutput = product.priceInput * 125 / 100;
                        _context.Add(existingInventory);
                    }
                    else
                    {
                        existingInventory.priceInput = product.priceInput;
                        existingInventory.priceOutput = product.priceInput * 125 / 100;
                       // existingInventory.totalPrice = existingInventory.priceOutput * existingInventory.Quantity;
                        _context.Update(existingInventory);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

        // GET: Products/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // POST: Products/Delete/5
        [Authorize(Roles = "ADMIN")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}

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
using StorageManagement_MVC.Models;

namespace StorageManagement_MVC.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly StorageManagement_MVCContext _context;

        public ReceiptsController(StorageManagement_MVCContext context)
        {
            _context = context;
        }

        // GET: Receipts
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index(string SearchString)
        {
            var ListReceipts = from rc in _context.Receipt select rc;
            if (!String.IsNullOrEmpty(SearchString))
            {
                DateTime searchDate;
                var isDate = DateTime.TryParse(SearchString, out searchDate);
                ListReceipts = ListReceipts.Where(s =>
                    s.Id!.Contains(SearchString) ||
                    s.userName!.Contains(SearchString) ||
                    (isDate && s.createdAt == searchDate) ||
                    s.createdAt!.ToString().Contains(SearchString));
            }
            return View(await ListReceipts.ToListAsync());
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ReceiptDetail(string SearchString)
        {
            var ListReceiptsDetail = from rc in _context.ReceiptDetail select rc;
            if (!String.IsNullOrEmpty(SearchString))
            {
                ListReceiptsDetail = ListReceiptsDetail.Where(s =>
                    s.idReceipt!.Contains(SearchString) ||
                    s.idProduct!.Contains(SearchString) ||
                    s.productName!.Contains(SearchString));
            }
            return View(await ListReceiptsDetail.ToListAsync());
        }

        private string GenerateNewReceiptId()
        {
            var lastReceipt = _context.Receipt.OrderByDescending(r => r.Id).FirstOrDefault();
            if (lastReceipt != null && lastReceipt.Id.StartsWith("RC"))
            {
                int lastIdNumber = int.Parse(lastReceipt.Id.Substring(2));
                return "RC" + (lastIdNumber + 1).ToString("D4");
            }
            else
            {
                return "RC0001";
            }
        }

        // GET: Receipts/Details/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // GET: Receipts/Create
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            var products = _context.Product.ToList(); 

            var viewModel = new AddReceiptPageViewModel
            {
                Products = products 
            };

            return View(viewModel);
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddReceiptPageViewModel viewModel)
        {
           
            bool isNull = false;
            foreach(var rcd in viewModel.ReceiptDetails)
            {
                if(rcd.idProduct == null)
                {
                    isNull = true;
                    ModelState.AddModelError("Receipt.userName", "Product is required");
                }
                if(rcd.quantity == null)
                {
                    isNull = true;
                    ModelState.AddModelError("Receipt.userName", "Product quantity is required");
                }
            }

            if (viewModel.Receipt.userName == null || isNull)
            {
                var products = _context.Product.ToList();
                viewModel.Products = products;
                return View(viewModel);
            }    

            viewModel.Receipt.Id = GenerateNewReceiptId();
            viewModel.Receipt.createdAt = DateTime.Now;
            viewModel.Receipt.totalProductQuantity = 0;
            viewModel.Receipt.totalPrice = 0;

            foreach (var detail in viewModel.ReceiptDetails)
            {
                
                detail.idReceipt = viewModel.Receipt.Id;
                

                 var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == detail.idProduct);
                var productInventory = await _context.productInventory.FirstOrDefaultAsync(p => p.Id == detail.idProduct);
                
                if (product != null && productInventory != null)
                {
                    detail.totalPrice = detail.quantity * product.priceOutput;
                    detail.productCategory = product.Category;
                    detail.productName = product.Name;
                    productInventory.Quantity += detail.quantity;
                    productInventory.totalPrice += detail.totalPrice;
                }
                viewModel.Receipt.totalProductQuantity += detail.quantity;
                viewModel.Receipt.totalPrice += detail.totalPrice;
                
                _context.Add(detail);
                _context.Update(productInventory);
            }
            
            _context.Add(viewModel.Receipt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Receipts/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,userName,totalProductQuantity,totalPrice,createdAt")] Receipt receipt)
        {
            if (id != receipt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.Id))
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
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [Authorize(Roles = "ADMIN")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt != null)
            {
                _context.Receipt.Remove(receipt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(string id)
        {
            return _context.Receipt.Any(e => e.Id == id);
        }
    }
}

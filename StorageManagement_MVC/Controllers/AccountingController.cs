using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageManagement_MVC.Data;
using StorageManagement_MVC.Models;
using StorageManagement_MVC.ViewModels;

namespace StorageManagement_MVC.Controllers
{
    public class AccountingController : Controller
    {
        private readonly StorageManagement_MVCContext _context;
        private readonly ILogger<AccountingController> _logger;
        public AccountingController(ILogger<AccountingController> logger, StorageManagement_MVCContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize(Roles = "ACCOUNTING")]
        public async Task<IActionResult> Index(string ProductListSearchString, string ProductInventorySearchString, string ReceiptsSearchString, string ReceiptDetailSearchString)
        {
            var products = from p in _context.Product select p;
            var receipts = from r in _context.Receipt select r;
            var productInventory = from pi in _context.productInventory select pi;
            var receiptDetails = from rd in _context.ReceiptDetail select rd;
            if (!String.IsNullOrEmpty(ProductListSearchString))
            {
                products = products.Where(s =>
                    s.Id!.Contains(ProductListSearchString) ||
                    s.Name!.Contains(ProductListSearchString));
            }

            if (!String.IsNullOrEmpty(ProductInventorySearchString))
            {
                productInventory = productInventory.Where(s =>
                    s.Id!.Contains(ProductInventorySearchString) ||
                    s.Name!.Contains(ProductInventorySearchString));
            }

            if (!String.IsNullOrEmpty(ReceiptsSearchString))
            {
                DateTime searchDate;
                var isDate = DateTime.TryParse(ReceiptsSearchString, out searchDate);
                receipts = receipts.Where(s =>
                    s.Id!.Contains(ReceiptsSearchString) ||
                    s.userName!.Contains(ReceiptsSearchString) ||
                    (isDate && s.createdAt == searchDate) ||
                    s.createdAt!.ToString().Contains(ReceiptsSearchString));
            }

            if (!String.IsNullOrEmpty(ReceiptDetailSearchString))
            {
                receiptDetails = receiptDetails.Where(s =>
                     s.idReceipt!.Contains(ReceiptDetailSearchString) ||
                     s.idProduct!.Contains(ReceiptDetailSearchString) ||
                     s.productName!.Contains(ReceiptDetailSearchString));
            }

            var viewModel = new AccountingVM
            {             
                Products = products,
                Receipts = receipts,
                ProductInventory = productInventory,
                ReceiptDetails = receiptDetails
            };

            return View(viewModel);
        }


    }
}

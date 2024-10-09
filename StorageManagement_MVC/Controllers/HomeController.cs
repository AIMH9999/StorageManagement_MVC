using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageManagement_MVC.Data;
using StorageManagement_MVC.Models;
using System.Diagnostics;

namespace StorageManagement_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StorageManagement_MVCContext _context;

        public HomeController(ILogger<HomeController> logger, StorageManagement_MVCContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            var totalProducts = await _context.Product.CountAsync();
            var totalReceipts = await _context.Receipt.CountAsync();
            var totalUsers = await _context.User.CountAsync();
            var totalQuantityProducts = _context.productInventory.Sum(p => p.Quantity ?? 0);
            //var products = await _context.Product.ToListAsync();
            //var receipts = await _context.Receipt.ToListAsync();

            var viewModel = new HomePageViewModel
            {
                TotalProducts = totalProducts,
                TotalReceipts = totalReceipts,
                TotalUsers = totalUsers,
                TotalQuantityProducts = totalQuantityProducts,
                //Products = products,
                //Receipts = receipts
            };

            return View(viewModel);
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

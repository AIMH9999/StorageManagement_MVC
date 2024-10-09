using StorageManagement.Models;

namespace StorageManagement_MVC.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Receipt> Receipts { get; set; }
        public IEnumerable<productInventory> ProductInventory { get; set; }
        public IEnumerable<User> Users { get; set; }
        public int TotalProducts { get; set; }
        public int TotalReceipts { get; set; }
        public int TotalQuantityProducts { get; set; }
        public int TotalUsers { get; set; }
    }
}

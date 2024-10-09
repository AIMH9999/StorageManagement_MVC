using StorageManagement.Models;

namespace StorageManagement_MVC.ViewModels
{
    public class AccountingVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Receipt> Receipts { get; set; }
        public IEnumerable<productInventory> ProductInventory { get; set; }
        public IEnumerable<ReceiptDetail> ReceiptDetails { get; set; }
    }
}

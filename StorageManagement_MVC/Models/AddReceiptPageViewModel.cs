using StorageManagement.Models;
namespace StorageManagement_MVC.Models
{
    public class AddReceiptPageViewModel
    {
        public string SelectedProductId { get; set; }
        public List<Product> Products { get; set; }
        public Receipt Receipt { get; set; }
        //public ReceiptDetail ReceiptDetail { get; set; }
        public List<ReceiptDetail> ReceiptDetails { get; set; }
        public string userName { get; set; }
    }
}

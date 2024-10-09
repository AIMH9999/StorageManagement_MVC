using System.ComponentModel.DataAnnotations;


namespace StorageManagement.Models
{
    public class ReceiptDetail
    {
        public int Id { get; set; }
        public string? idReceipt { get; set; }
        public string? idProduct { get; set; }

        [Required(ErrorMessage = "productName is required.")]
        public string? productName { get; set; }
        public string? productCategory { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        public int? quantity { get; set; }
        public long? totalPrice { get; set; }
    }  
}

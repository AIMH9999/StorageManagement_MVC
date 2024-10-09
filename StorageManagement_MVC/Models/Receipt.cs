using System.ComponentModel.DataAnnotations;

namespace StorageManagement.Models
{
    public class Receipt
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string? userName { get; set; }
        public int? totalProductQuantity { get; set; }
        public long? totalPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime? createdAt { get; set; }
    }
}

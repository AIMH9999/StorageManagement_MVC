using System.ComponentModel.DataAnnotations;

namespace StorageManagement.Models
{
    public class Product
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Input price is required.")]
        public int? priceInput { get; set; }
        public int? priceOutput { get; set; }
    }
}

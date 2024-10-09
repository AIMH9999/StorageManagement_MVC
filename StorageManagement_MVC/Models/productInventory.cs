namespace StorageManagement.Models
{
    public class productInventory
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int? priceInput { get; set; }
        public int? Quantity { get; set; }
        public int? priceOutput { get; set; }
        public long? totalPrice { get; set; }
    }
}

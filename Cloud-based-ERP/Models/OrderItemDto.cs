namespace Cloud_based_ERP.Models
{
    public class OrderItemDto
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

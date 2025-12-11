namespace Cloud_based_ERP.Models
{
    public class OrderRequest
    {
        public Guid RequestId { get; set; }
        public CustomerDto? Customer { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }
}

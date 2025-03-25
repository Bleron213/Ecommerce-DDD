namespace Ecommerce.API.Contracts.Request.Order
{
    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

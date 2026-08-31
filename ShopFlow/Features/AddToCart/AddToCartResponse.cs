namespace ShopFlow.Features.AddToCart
{
    public sealed class AddToCartResponse
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int NewCartItemCount { get; set; }
        public decimal CartTotal { get; set; }
    }
}

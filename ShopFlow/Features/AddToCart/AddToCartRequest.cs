using MediatR;

namespace ShopFlow.Features.AddToCart
{
    public sealed class AddToCartRequest : IRequest<AddToCartResponse>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

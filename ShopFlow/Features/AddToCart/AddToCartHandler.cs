using FluentValidation;
using MediatR;

namespace ShopFlow.Features.AddToCart
{
    public sealed class AddToCartHandler : IRequestHandler<AddToCartRequest, AddToCartResponse>
    {
        private readonly IValidator<AddToCartRequest> _validator;
        private readonly IProductCatalog _productCatalog;
        private readonly ICartStore _cartStore;

        public AddToCartHandler(IValidator<AddToCartRequest> validator,
            IProductCatalog productCatalog,
            ICartStore cartStore)
        {
            _validator = validator;
            _productCatalog = productCatalog;
            _cartStore = cartStore;
        }

        public async Task<AddToCartResponse> Handle(AddToCartRequest request, CancellationToken cancellationToken)
        {
            // Validate — throws FluentValidation.ValidationException on failure.
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Business logic
            var product = await _productCatalog.FindAsync(request.ProductId, cancellationToken) 
                ?? throw new ProductNotFoundException(request.ProductId);

            var cart = await _cartStore.AddItemAsync(
                request.CartId,
                request.ProductId,
                request.Quantity,
                product.UnitPrice,
                cancellationToken);

            // Respond
            return new AddToCartResponse
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                NewCartItemCount = cart.TotalItemCount,
                CartTotal = cart.Total
            };


        }
    }
    // Feature-local exceptions and collaborator contracts
    public sealed class ProductNotFoundException(Guid productId) 
        : Exception($"Product {productId} does not exist ");

    public sealed class ProductOutOfStockException(string productName)
        : Exception($"Product '{productName}' is currently out of stock");

    public interface IProductCatalog
    {
        Task<ProductLookup?> FindAsync(Guid productId, CancellationToken cancellationToken);
    }

    public sealed class ProductLookup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public bool IsInStock { get; set; }

    }

    public interface ICartStore
    {
        Task<CartSnapshot> AddItemAsync(
            Guid cartId,
            Guid productId,
            int quantity,
            decimal unitPrice,
            CancellationToken cancellationToken);
    }

    public sealed class CartSnapshot
    {
        public Guid Id { get; set; }
        public int TotalItemCount { get; set; }
        public decimal Total { get; set; }
    }


}

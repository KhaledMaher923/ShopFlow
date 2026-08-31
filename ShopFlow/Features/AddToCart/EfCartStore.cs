using ShopFlow.Data;
using ShopFlow.Enitities;
using Microsoft.EntityFrameworkCore;


namespace ShopFlow.Features.AddToCart
{
    public sealed class EfCartStore : ICartStore
    {
        private readonly ShopFlowDbContext _dbContext;

        public EfCartStore(ShopFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CartSnapshot> AddItemAsync(
            Guid cartId,
            Guid productId,
            int quantity,
            decimal unitPrice,
            CancellationToken cancellationToken)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(
                    c => c.Id == cartId,
                    cancellationToken);

            if (cart is null)
            {
                throw new InvalidOperationException(
                    $"Cart {cartId} not found.");
            }

            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                });
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var totalItemCount = cart.Items
                .Sum(i => i.Quantity);

            var total = cart.Items
                .Sum(i => i.Quantity * i.UnitPrice);

            return new CartSnapshot
            {
                Id = cart.Id,
                TotalItemCount = totalItemCount,
                Total = total
            };
        }
    }
}
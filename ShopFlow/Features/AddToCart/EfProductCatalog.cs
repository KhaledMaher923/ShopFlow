using Microsoft.EntityFrameworkCore;
using ShopFlow.Data;

namespace ShopFlow.Features.AddToCart
{
    public sealed class EfProductCatalog : IProductCatalog
    {
        private readonly ShopFlowDbContext _dbContext;

        public EfProductCatalog(ShopFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductLookup?> FindAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => new ProductLookup
                {
                    Id = p.Id,
                    Name = p.Name,
                    UnitPrice = p.UnitPrice,
                    IsInStock = p.IsInStock
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

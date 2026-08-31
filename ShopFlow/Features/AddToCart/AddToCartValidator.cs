using FluentValidation;

namespace ShopFlow.Features.AddToCart
{
    public sealed class AddToCartValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity Must be At Least 1")
                .LessThanOrEqualTo(100)
                .WithMessage("Quantity Cannot Exceed 100 per request");

        }

    }
}

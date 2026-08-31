using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopFlow.Features.AddToCart
{
    [ApiController]
    [Route("api/cart")]
    public sealed class AddToCartController : ControllerBase
    {
        private readonly ISender _sender;
        public AddToCartController(ISender sender)
        {
            _sender = sender;
        }

        // POST /api/cart/items
        [HttpPost("items")]
        [ProducesResponseType(typeof(AddToCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddItem(
            [FromBody] AddToCartRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _sender.Send(request, cancellationToken);
                return Ok(response);
            }
            catch(ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return ValidationProblem(ModelState);
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ProductOutOfStockException ex)
            {
                return Conflict(new { message = ex.Message });
            }

        }

    }
}

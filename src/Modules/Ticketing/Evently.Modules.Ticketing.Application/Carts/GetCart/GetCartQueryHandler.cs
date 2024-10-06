using Evently.Common.Application.Messaging;
using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Application.Carts.GetCart;

internal sealed class GetCartQueryHandler(CartService cartService) : IQueryHandler<GetCartQuery, Cart>
{
    public async  Task<ResponseWrapper<Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart= await cartService.GetAsync(request.CustomerId, cancellationToken);
        return ResponseWrapper<Cart>.Success(cart);
    }
}
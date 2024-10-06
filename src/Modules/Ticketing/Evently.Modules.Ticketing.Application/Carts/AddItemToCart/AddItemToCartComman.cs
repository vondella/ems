using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.PublicApi;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Users.PublicApi;
using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(Guid CustomerId, Guid TicketTypeId, decimal Quantity) : ICommand;
internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.TicketTypeId).NotEmpty();
        RuleFor(c => c.Quantity).GreaterThan(decimal.Zero);
    }
}
internal sealed class AddItemToCartCommandHandler(CartService cartService, ICustomerRepository customerRepository, IEventsApi eventsApi)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<ResponseWrapper> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get customer
     var  customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return ResponseWrapper<Guid>.Fail(CustomerErrors.NotFound(request.CustomerId));
        }

        // 2. Get ticket type
        var ticketType = await eventsApi.GetTicketTypeAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return ResponseWrapper<Guid>.Fail(TicketErrors.NotFound(request.TicketTypeId));
        }

        // 3. Add item to cart
        var cartItem = new CartItem
        {
            TicketTypeId = ticketType.ResponseData.Id,
            Price = ticketType.ResponseData.Price,
            Quantity = request.Quantity,
            Currency = ticketType.ResponseData.Currency
        };

        await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return ResponseWrapper<Guid>.Success(request.TicketTypeId);
    }
}
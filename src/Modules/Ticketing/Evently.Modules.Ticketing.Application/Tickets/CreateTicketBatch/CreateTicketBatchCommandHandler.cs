using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
internal sealed class CreateTicketBatchCommandHandler(
    IOrderRepository orderRepository,
    ITicketTypeRepository ticketTypeRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketBatchCommand>
{
    public async  Task<ResponseWrapper> Handle(CreateTicketBatchCommand request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository.GetAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return ResponseWrapper<Guid>.Fail(TicketErrors.NotFound(request.OrderId));
        }

        var result = order.IssueTickets();

        if (!result.IsSuccessful)
        {
            return ResponseWrapper<Guid>.Fail(TicketErrors.NotFound(request.OrderId));
        }

        List<Ticket> tickets = [];
        foreach (OrderItem orderItem in order.OrderItems)
        {
            TicketType? ticketType = await ticketTypeRepository.GetAsync(orderItem.TicketTypeId, cancellationToken);

            if (ticketType is null)
            {
                return ResponseWrapper<Guid>.Fail(TicketErrors.NotFound(ticketType.Id));
            }

            for (int i = 0; i < orderItem.Quantity; i++)
            {
                var ticket = Ticket.Create(order, ticketType);

                tickets.Add(ticket);
            }
        }

        ticketRepository.InsertRange(tickets);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Guid>.Success();
    }
}
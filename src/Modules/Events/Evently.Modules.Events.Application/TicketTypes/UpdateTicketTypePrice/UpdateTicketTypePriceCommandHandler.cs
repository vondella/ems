using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandHandler(
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTicketTypePriceCommand>
{
    public async  Task<ResponseWrapper> Handle(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken)
    {
        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);
        if (ticketType is null)
        {
            return ResponseWrapper<Guid>.Fail($"No ticket with id {request.TicketTypeId} is found");
        }

        ticketType.UpdatePrice(request.Price);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Guid>.Success(ticketType.Id);
    }
}
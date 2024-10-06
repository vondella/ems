using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

internal sealed class PublishEventCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PublishEventCommand>
{
    public async Task<ResponseWrapper> Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
        {
            return ResponseWrapper<Event>.Fail(EventErrors.NotFound(@event.Id));
        }
        if (!await ticketTypeRepository.ExistsAsync(@event.Id, cancellationToken))
        {
            return ResponseWrapper<Event>.Fail(EventErrors.NoTicketsFound);
        }

        var result = @event.Publish();

        if (!result.IsSuccessful)
        {
            return ResponseWrapper<Event>.Fail(EventErrors.NotDraft);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Event>.Success();
    }
}
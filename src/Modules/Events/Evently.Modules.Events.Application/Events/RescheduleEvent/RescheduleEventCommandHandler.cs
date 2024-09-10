using Evently.Modules.Events.Application.Abstractions.Clock;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RescheduleEventCommand>
{
    public async Task<ResponseWrapper> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
        {
            return ResponseWrapper<Event>.Fail($"Evnt with Id {request.EventId}");
        }

        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return ResponseWrapper<Event>.Fail("invalid date range");
        }

        @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Event>.Success();
    }
}
using Evently.Common.Application.Clock;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed  class CancelEventCommandHandler(IDateTimeProvider dateTimeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CancelEventCommand>
{
    public async Task<ResponseWrapper> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return ResponseWrapper<Event>.Fail(EventErrors.NotFound(@event.Id));
        }
        var result = @event.Cancel(dateTimeProvider.UtcNow);

        if (!result.IsSuccessful)
        {
            return ResponseWrapper<Event>.Fail(EventErrors.NotFound(@event.Id));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return   ResponseWrapper<Event>.Success();
    }
}
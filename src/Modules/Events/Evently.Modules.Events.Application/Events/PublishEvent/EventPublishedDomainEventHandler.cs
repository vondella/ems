using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events.GetEvent;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

internal sealed class EventPublishedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<EventPublishedDomainEvent>
{
    public override async  Task Handle(EventPublishedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
       var  result = await sender.Send(new GetEventQuery(domainEvent.EventId), cancellationToken);

        if (!result.IsSuccessful)
        {
            throw new EventlyException(nameof(GetEventQuery));
        }

        await eventBus.PublishAsync(
            new EventPublishedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccuredOnUtc,
                result.ResponseData.Id,
                result.ResponseData.Title,
                result.ResponseData.Description,
                result.ResponseData.Location,
                result.ResponseData.StartsAtUtc,
                result.ResponseData.EndsAtUtc,
                result.ResponseData.TicketTypes.Select(t => new TicketTypeModel
                {
                    Id = t.TicketTypeId,
                    EventId = result.ResponseData.Id,
                    Name = t.Name,
                    Price = t.Price,
                    Currency = t.Currency,
                    Quantity = t.Quantity
                }).ToList()),
            cancellationToken);
    }
}
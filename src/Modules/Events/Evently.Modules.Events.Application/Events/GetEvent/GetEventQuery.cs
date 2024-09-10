using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

public record GetEventQuery(Guid EventId) : IRequest<EventResponse?>;
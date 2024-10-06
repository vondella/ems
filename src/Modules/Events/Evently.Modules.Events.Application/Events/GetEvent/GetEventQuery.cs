using Evently.Common.Application.Messaging;
using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

public record GetEventQuery(Guid EventId) : IQuery<EventResponse?>;
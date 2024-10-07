using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Users.IntegrationEvent;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

internal sealed class UserRegisteredIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async  Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (!result.IsSuccessful)
        {
            throw new EventlyException(nameof(CreateAttendeeCommand));
        }
    }
}
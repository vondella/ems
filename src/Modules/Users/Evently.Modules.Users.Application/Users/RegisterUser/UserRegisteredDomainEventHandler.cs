using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.PublicApi;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Domain;
using Evently.Modules.Users.IntegrationEvent;
using MediatR;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async  Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        ResponseWrapper<UserResponse> result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (!result.IsSuccessful)
        {
            throw new EventlyException(nameof(GetUserQuery));
        }

        //await ticketingApi.CreateCustomerAsync(
        //    result.ResponseData.Id,
        //    result.ResponseData.Email,
        //    result.ResponseData.FirstName,
        //    result.ResponseData.LastName,
        //    cancellationToken);

        await eventBus.PublishAsync(new UserRegisteredIntegrationEvent(
                notification.UserId,
                notification.OccuredOnUtc,
            result.ResponseData.Id,
            result.ResponseData.Email,
            result.ResponseData.FirstName,
            result.ResponseData.LastName),
            cancellationToken);

    }
}
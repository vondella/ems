using Evently.Common.Domain;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.PublicApi;
using MediatR;
using TicketTypeResponse = Evently.Modules.Events.PublicApi.TicketTypeResponse;

namespace Evently.Modules.Events.Infrastracture.PublicApi;

internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<ResponseWrapper<TicketTypeResponse?>> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        ResponseWrapper<Application.TicketTypes.GetTicketType.TicketTypeResponse> result =
            await sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);

        if (!result.IsSuccessful)
        {
            return null;
        }

        return ResponseWrapper<TicketTypeResponse>.Success(new TicketTypeResponse(
            result.ResponseData.Id,
            result.ResponseData.EventId,
            result.ResponseData.Name,
            result.ResponseData.Price,
            result.ResponseData.Currency,
            result.ResponseData.Quantity));
    }
}
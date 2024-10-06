using Evently.Common.Domain;

namespace Evently.Modules.Events.PublicApi;

public interface IEventsApi
{
    Task<ResponseWrapper<TicketTypeResponse?>> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default);
}
﻿using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using System.Data.Common;
using Dapper;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketType;

internal sealed class GetTicketTypeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypeQuery, TicketTypeResponse>
{
    public async  Task<ResponseWrapper<TicketTypeResponse>> Handle(GetTicketTypeQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(TicketTypeResponse.Id)},
                 event_id AS {nameof(TicketTypeResponse.EventId)},
                 name AS {nameof(TicketTypeResponse.Name)},
                 price AS {nameof(TicketTypeResponse.Price)},
                 currency AS {nameof(TicketTypeResponse.Currency)},
                 quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.ticket_types
             WHERE id = @TicketTypeId
             """;

        TicketTypeResponse? ticketType =
            await connection.QuerySingleOrDefaultAsync<TicketTypeResponse>(sql, request);

        if (ticketType is null)
        {
            return ResponseWrapper<TicketTypeResponse>.Fail($"Ticket Type with Id {request.TicketTypeId} is not found");
        }

        return ResponseWrapper<TicketTypeResponse>.Success(ticketType);
    }
}
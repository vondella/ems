﻿using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions.Data;
using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory connectionFactory) : IRequestHandler<GetEventQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await connectionFactory.OpenConnectionAsync();
        const string sql = $"""
                          SELECT
                           id as {nameof(EventResponse.Id)},
                           title as {nameof(EventResponse.Title)},
                           description as {nameof(EventResponse.Description)},
                           location as {nameof(EventResponse.Location)},
                           starts_at_utc as {nameof(EventResponse.StartsAtUtc)},
                           ends_at_utc as {nameof(EventResponse.EndsAtUtc)}
                           FROM event.events
                           WHERE id=@EventId
                          """;
        EventResponse? @event = await connection.QuerySingleAsync(sql, request);
        return @event;
    }
}

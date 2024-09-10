﻿using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastracture.Events;

internal sealed class EventRepository(EventsDbContext context):IEventRepository
{
    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
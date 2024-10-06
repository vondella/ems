using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastracture.Events;

internal sealed class EventRepository(AttendanceDbContext context) : IEventRepository
{
    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
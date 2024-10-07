using Evently.Modules.Attendance.Domain.Attendee;
using Evently.Modules.Attendance.Infrastracture.Database;

namespace Evently.Modules.Attendance.Infrastracture.Attendees;

internal sealed class AttendeeRepository(AttendanceDbContext context) : IAttendeeRepository
{
    public Task<Attendee?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Insert(Attendee attendee)
    {
        throw new NotImplementedException();
    }
}
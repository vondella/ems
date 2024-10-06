namespace Evently.Modules.Attendance.Domain.Attendee;

public interface IAttendeeRepository
{
    Task<Attendee?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Attendee attendee);
}
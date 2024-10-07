namespace Evently.Modules.Attendance.Infrastracture.Inbox;


internal sealed class InboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}
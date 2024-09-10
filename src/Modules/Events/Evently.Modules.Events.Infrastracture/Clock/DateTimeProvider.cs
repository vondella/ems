using Evently.Modules.Events.Application.Abstractions.Clock;

namespace Evently.Modules.Events.Infrastracture.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
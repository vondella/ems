using Evently.Common.Application.Clock;

namespace Evently.Common.Infrastracture.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
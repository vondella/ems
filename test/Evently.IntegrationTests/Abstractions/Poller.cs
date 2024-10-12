using Evently.Common.Domain;
using static MassTransit.ValidationResultExtensions;

namespace Evently.IntegrationTests.Abstractions;

internal static class Poller
{
    private static readonly Error Timeout = Error.Failure("Poller.Timeout", "The poller has time out");
    internal static async Task<ResponseWrapper<T>> WaitAsync<T>(TimeSpan timeout, Func<Task<ResponseWrapper<T>>> func)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        DateTime endTimeUtc = DateTime.UtcNow.Add(timeout);
        while (DateTime.UtcNow < endTimeUtc && await timer.WaitForNextTickAsync())
        {
            var result = await func();

            if (result.IsSuccessful)
            {
                return result;
            }
        }

        return ResponseWrapper<T>.Fail(Timeout);
    }

}
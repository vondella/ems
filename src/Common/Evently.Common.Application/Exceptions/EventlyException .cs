using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evently.Common.Application.Exceptions;

public sealed class EventlyException : Exception
{
    public EventlyException(string requestName, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        //Error = error;
    }

    public string RequestName { get; }

    //public Error? Error { get; }
}
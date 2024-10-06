namespace Evently.Common.Domain;

public interface IResponseWrapper
{
    List<string> Messages { get; set; }
    bool IsSuccessful { get; set; } 
    Error Error { get; set; }
}

public interface IResponseWrapper<out T> : IResponseWrapper
{
    T ResponseData { get; }
}

namespace Evently.Common.Domain;


public class ResponseWrapper : IResponseWrapper
{
    public List<string> Messages { get; set; }
    public bool IsSuccessful { get; set; }
    public Error Error { get; set; }

    public static IResponseWrapper Fail(Error error)
    {
        return new ResponseWrapper { IsSuccessful = false,Error = error};
    }

    public static IResponseWrapper Fail(string message, Error error)
    {
        return new ResponseWrapper { IsSuccessful = false, Messages = new List<string> { message },Error = error};
    }

    public static IResponseWrapper Fail(List<string> messages,Error error)
    {
        return new ResponseWrapper { IsSuccessful = false, Messages = messages,Error = error};
    }

    public static Task<IResponseWrapper> FailAsync(Error error)
    {
        return Task.FromResult(Fail(error));
    }

    public static Task<IResponseWrapper> FailAsync(string message,Error error)
    {
        return Task.FromResult(Fail(message,error));
    }

    public static Task<IResponseWrapper> FailAsync(List<string> messages,Error error)
    {
        return Task.FromResult(Fail(messages,error));
    }

    public static IResponseWrapper Success()
    {
        return new ResponseWrapper { IsSuccessful = true,Error = Error.None};
    }

    public static IResponseWrapper Success(string message)
    {
        return new ResponseWrapper { IsSuccessful = true, Messages = new List<string> { message }, Error = Error.None };
    }

    public static Task<IResponseWrapper> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<IResponseWrapper> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}
public class ResponseWrapper<T> : ResponseWrapper, IResponseWrapper<T>
{
    public ResponseWrapper()
    {
    }

    public T ResponseData { get; set; }

    public new static ResponseWrapper<T> Fail(Error error)
    {
        return new ResponseWrapper<T> { IsSuccessful = false,Error = error};
    }

    public new static ResponseWrapper<T> Fail(string message,Error error)
    {
        return new ResponseWrapper<T> { IsSuccessful = false, Messages = new List<string> { message },Error = error};
    }

    public new static ResponseWrapper<T> Fail(List<string> messages,Error error)
    {
        return new ResponseWrapper<T> { IsSuccessful = false, Messages = messages, Error = error};
    }

    public new static Task<ResponseWrapper<T>> FailAsync(Error error)
    {
        return Task.FromResult(Fail(error));
    }

    public new static Task<ResponseWrapper<T>> FailAsync(string message,Error error)
    {
        return Task.FromResult(Fail(message,error));
    }

    public new static Task<ResponseWrapper<T>> FailAsync(List<string> messages,Error error)
    {
        return Task.FromResult(Fail(messages,error));
    }

    public new static ResponseWrapper<T> Success()
    {
        return new ResponseWrapper<T> { IsSuccessful = true,Error = Error.None};
    }

    public new static ResponseWrapper<T> Success(string message)
    {
        return new ResponseWrapper<T> { IsSuccessful = true, Messages = new List<string> { message }, Error = Error.None };
    }

    public static ResponseWrapper<T> Success(T data)
    {
        return new ResponseWrapper<T> { IsSuccessful = true, ResponseData = data, Error = Error.None };
    }

    public static ResponseWrapper<T> Success(T data, string message)
    {
        return new ResponseWrapper<T> { IsSuccessful = true, ResponseData = data, Messages = new List<string> { message }, Error = Error.None };
    }

    public static ResponseWrapper<T> Success(T data, List<string> messages)
    {
        return new ResponseWrapper<T> { IsSuccessful = true, ResponseData = data, Messages = messages, Error = Error.None };
    }

    public new static Task<ResponseWrapper<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<ResponseWrapper<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<ResponseWrapper<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<ResponseWrapper<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
}
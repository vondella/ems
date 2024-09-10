﻿namespace Evently.Modules.Events.Domain.Abstractions;

public interface IResponseWrapper
{
    List<string> Messages { get; set; }
    bool IsSuccessful { get; set; }
}

public interface IResponseWrapper<out T> : IResponseWrapper
{
    T ResponseData { get; }
}

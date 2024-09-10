using Evently.Modules.Events.Domain.Abstractions;
using MediatR;

namespace Evently.Modules.Events.Application.Abstractions.Messaging;


public interface ICommand : IRequest<ResponseWrapper>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<ResponseWrapper<TResponse>>, IBaseCommand;

public interface IBaseCommand;
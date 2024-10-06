using Evently.Common.Domain;
using MediatR;

namespace Evently.Common.Application.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ResponseWrapper>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ResponseWrapper<TResponse>>
    where TCommand : ICommand<TResponse>;
using Evently.Common.Domain;
using MediatR;

namespace Evently.Common.Application.Messaging;


public interface ICommand : IRequest<ResponseWrapper>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<ResponseWrapper<TResponse>>, IBaseCommand;

public interface IBaseCommand;
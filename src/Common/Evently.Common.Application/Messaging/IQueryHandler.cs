using Evently.Common.Domain;
using MediatR;
namespace Evently.Common.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ResponseWrapper<TResponse>>
    where TQuery : IQuery<TResponse>;
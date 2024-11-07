using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, BaseResult<TResponse>>
    where TQuery : IQuery<TResponse> { }

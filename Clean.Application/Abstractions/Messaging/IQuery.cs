using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<BaseResult<TResponse>> { }

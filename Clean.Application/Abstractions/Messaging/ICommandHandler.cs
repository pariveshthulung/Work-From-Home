using System;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, BaseResult<TResponse>>
    where TCommand : ICommand<TResponse> { }

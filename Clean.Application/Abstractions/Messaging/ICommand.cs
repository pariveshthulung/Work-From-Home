using System;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Abstractions.Messaging;

public interface ICommand : IRequest<BaseResult> { }

public interface ICommand<TResponse> : IRequest<BaseResult<TResponse>> { }

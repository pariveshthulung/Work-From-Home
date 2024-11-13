using System;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries.ManagersEmail;

public class GetManagersEmailQuery : IRequest<BaseResult<List<string>>> { }

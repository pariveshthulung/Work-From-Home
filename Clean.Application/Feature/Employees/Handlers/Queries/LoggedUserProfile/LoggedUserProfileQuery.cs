using Clean.Application.Dto.Employee;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Commands.LoggedUserProfile;

public class LoggedUserProfileQuery : IRequest<BaseResult<EmployeeDto>> { }

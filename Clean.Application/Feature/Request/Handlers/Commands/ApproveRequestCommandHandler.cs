using AutoMapper;
using Clean.Application.Dto.Approval.Validation;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Handlers.Commands;

public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand, BaseResult<int>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;

    public ApproveRequestCommandHandler(
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        ICurrentUserService currentUser
    )
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
        _currentUser = currentUser;
    }

    public async Task<BaseResult<int>> Handle(
        ApproveRequestCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var validator = new ApprovalDtoValidation(_employeeRepository);
            var validationResult = await validator.ValidateAsync(
                command.ApproveRequestDto,
                cancellationToken
            );
            if (!validationResult.IsValid)
            {
                var errors = validationResult
                    .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage))
                    .ToList();
                return BaseResult<int>.Failure(errors);
            }
            var employee = await _employeeRepository.GetEmployeeByGuidIdAsync(
                command.ApproveRequestDto.EmployeeId,
                cancellationToken
            );
            if (employee == null)
                return BaseResult<int>.Failure(EmployeeErrors.NotFound());
            var existingEmployee = employee;
            var request = employee.Requests.FirstOrDefault(x =>
                x.GuidId == command.ApproveRequestDto.RequestId
            );

            if (request == null)
                return BaseResult<int>.Failure(RequestErrors.NotFound());
            var currentUser = await _employeeRepository.GetEmployeeByEmailAsync(
                _currentUser.UserEmail,
                cancellationToken
            );
            if (currentUser is null)
                return BaseResult<int>.Failure(EmployeeErrors.Unauthorize());

            currentUser.ApproveRequest(request, command.ApproveRequestDto.ApprovalStatusId);

            await _employeeRepository.UpdateEmployeeAsync(
                employee,
                existingEmployee,
                cancellationToken
            );

            return BaseResult<int>.Ok(request.Id);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}

using AutoMapper;
using Clean.Application.Dto.Request.Validation;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using Clean.Domain.Enums;
using MediatR;

namespace Clean.Application.Feature.Requests.Handlers.Commands;

public class SubmitRequestCommandHandler : IRequestHandler<SubmitRequestCommand, BaseResult<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICurrentUserService _currentUserService;

    public SubmitRequestCommandHandler(
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResult<Guid>> Handle(
        SubmitRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        //validate user input
        var validator = new CreateRequestDtoValidator(_employeeRepository);
        var validationResult = await validator.ValidateAsync(
            request.CreateRequestDto,
            cancellationToken
        );
        if (!validationResult.IsValid)
        {
            var errors = validationResult
                .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage.ToString()))
                .ToList();
            return BaseResult<Guid>.Failure(errors);
        }
        //1. check requestedTo user
        var requestedTo = await _employeeRepository.GetEmployeeByEmailAsync(
            request.CreateRequestDto.RequestedToEmail,
            cancellationToken
        );
        if (requestedTo is null)
            return BaseResult<Guid>.Failure(
                EmployeeErrors.NotFound(request.CreateRequestDto.RequestedToEmail)
            );

        request.CreateRequestDto.RequestedTo = requestedTo.Id;

        //2.get current user
        var currentUser = await _employeeRepository.GetEmployeeByEmailAsync(
            _currentUserService.UserEmail!,
            cancellationToken
        );
        if (currentUser is null)
        {
            return BaseResult<Guid>.Failure(EmployeeErrors.Unauthorize());
        }

        if (currentUser.Id == requestedTo.Id)
            return BaseResult<Guid>.Failure(
                new Error(400, "Request.Submit", "Can't submit to own email")
            );
        var existingEmployee = currentUser;

        //3.check previous pending request
        var previousRequestExist = currentUser.Requests.Any(x =>
            x.Approval!.ApprovalStatusId == ApprovalStatusEnum.Pending.Id
        );
        if (previousRequestExist)
            return BaseResult<Guid>.Failure(
                new Error(
                    400,
                    "Request.Submit",
                    "Your previous request is pending so can't summit new request."
                )
            );

        var toRequest = _mapper.Map<GeneralRequest>(request.CreateRequestDto);

        //4.submit request
        currentUser.SubmitRequest(toRequest);
        await _employeeRepository.UpdateEmployeeAsync(
            currentUser,
            existingEmployee,
            cancellationToken
        );
        // await _employeeRepository.SaveChangesAsync(cancellationToken);
        return BaseResult<Guid>.Ok(toRequest.GuidId);
    }
}

using Clean.Application.Persistence.Contract;
using Clean.Domain.Enums;
using FluentValidation;

namespace Clean.Application.Dto.Approval.Validation;

public class ApprovalDtoValidation : AbstractValidator<ApproveRequestDto>
{
    public readonly IEmployeeRepository _employeeRepository;

    public ApprovalDtoValidation(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        // RuleFor(e => e.ApproverId)
        //     .NotEmpty()
        //     .WithMessage("{PropertyName} is required")
        //     .MustAsync(
        //         async (id, CancellationToken) =>
        //         {
        //             var result = await _employeeRepository.EmployeeExistAsync(
        //                 id,
        //                 CancellationToken
        //             );
        //             return result;
        //         }
        //     )
        //     .WithMessage("{PropertyName} doesnot exist.");
        RuleFor(e => e.RequestId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required");
        RuleFor(e => e.EmployeeId).NotEmpty().WithMessage("{PropertyName} is required");
        // .MustAsync(
        //     async (id, CancellationToken) =>
        //     {
        //         var result = await _employeeRepository.EmployeeExistAsync(
        //             id,
        //             CancellationToken
        //         );
        //         return result;
        //     }
        // )
        // .WithMessage("{PropertyName} doesnot exist.");

        RuleFor(e => e.ApprovalStatusId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Must(id => ApprovalStatusEnum.Statuses.Any(x => x.Id == id))
            .WithMessage("{PropertyName} id is invalid");
    }
}

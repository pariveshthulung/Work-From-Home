using Clean.Application.Persistence.Contract;
using Clean.Domain.Enums;
using FluentValidation;

namespace Clean.Application.Dto.Request.Validation;

public class CreateRequestDtoValidator : AbstractValidator<CreateRequestDto>
{
    public readonly IEmployeeRepository _employeeRepository;

    public CreateRequestDtoValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        // RuleFor(e => e.RequestedTo)
        //     .NotEmpty()
        //     .WithMessage("{PropertyName} is required.")
        //     .GreaterThan(0)
        //     .WithMessage("{PropertyName} id should be greater than {ComparisionValue}");
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
        ;
        RuleFor(e => e.FromDate)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .LessThan(e => e.ToDate)
            .WithMessage("{PropertyName} should be less than {ComparisionValue}");
        RuleFor(e => e.ToDate)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .GreaterThan(e => e.FromDate)
            .WithMessage("{PropertyName} should be greater than {ComparisionValue}");
        RuleFor(e => e.RequestedTypeId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Must(id => RequestTypeEnum.RequestTypes.Any(type => type.Id == id))
            .WithMessage("{PropertyName} is invalid.");
    }
}

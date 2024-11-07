using System;
using System.Data;
using FluentValidation;

namespace Clean.Application.Dto.Employee.Validation;

public class UpdateEmployeeDtoValitator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValitator()
    {
        Include(new IEmployeeDtoValidator());
        RuleFor(e => e.Id).NotEmpty().WithMessage("{PropertyName} is required");
        // .GreaterThan(0)
        // .WithMessage("{PropertyName} should be greater than {ComparisionValue}");
    }
}

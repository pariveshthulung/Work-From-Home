using Clean.Domain.Enums;
using FluentValidation;

namespace Clean.Application.Dto.Employee.Validation;

public class IEmployeeDtoValidator : AbstractValidator<IEmployeeDto>
{
    public IEmployeeDtoValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(30)
            .WithMessage("{PropertyName} should not exceed {ComparisionValue}");
        RuleFor(e => e.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithMessage("{PropertyName} must be a valid email address.");
        RuleFor(e => e.PhoneNumber)
            .NotEmpty()
            .WithMessage("{PropertyName} should not be empty.")
            .Matches(@"([0-9]{10})$")
            .WithMessage("{PropertyName} should contain 10 digits.");
        RuleFor(e => e.UserRoleId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than {ComparisionValue}")
            .Must(id => UserRoleEnum.Roles.Any(role => role.Id == id))
            .WithMessage("{PropertName} is not valid role.");
        RuleFor(e => e.Address.City).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(e => e.Address.Street).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(e => e.Address.PostalCode).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}

using System;
using Clean.Application.Dto.Employee;
using Clean.Application.Dto.Employee.Validation;
using FluentValidation;

namespace Clean.Application.Feature.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterEmployeeDto>
{
    public RegisterCommandValidator()
    {
        Include(new IEmployeeDtoValidator());
    }
}

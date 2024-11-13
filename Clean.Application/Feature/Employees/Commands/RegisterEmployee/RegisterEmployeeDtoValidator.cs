using Clean.Application.Persistence.Contract;
using FluentValidation;

namespace Clean.Application.Dto.Employee.Validation;

public class RegisterEmployeeDtoValidator : AbstractValidator<RegisterEmployeeDto>
{
    private readonly IEmployeeRepository _empRepo;

    public RegisterEmployeeDtoValidator(IEmployeeRepository employeeRepository)
    {
        _empRepo = employeeRepository;
        Include(new IEmployeeDtoValidator());
        RuleFor(e => e.Email)
            .MustAsync(
                async (email, token) =>
                {
                    var emp = await _empRepo.GetEmployeeByEmailAsync(email, token);
                    return emp is null ? true : false;
                }
            )
            .WithMessage("Email already exist!");
    }
}

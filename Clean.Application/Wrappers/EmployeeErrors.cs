namespace Clean.Application.Wrappers;

public class EmployeeErrors
{
    public static Error NotFound(Guid guidId) =>
        new(404, "Employee.NotFound", $"The Employee with Id = '{guidId}' was not found. ");

    public static Error NotFound() =>
        new(404, "Employee.NotFound", $"The employee was not found. ");

    public static Error NotFound(int id) =>
        new(404, "Employee.NotFound", $"The employee with Id = '{id}' was not found. ");

    public static Error NotFound(string email) =>
        new(404, "Employee.NotFound", $"The employee with email '{email}' was not found. ");

    public static Error Unauthorize() =>
        new(401, "Employee.Unauthorize", "The employee is unauthorized.");

    public static Error Forbiden() =>
        new(403, "Employee.Forbiden", $"You don't have permission to access the resources. ");

    public static Error EmailNotUnique(string email) =>
        new(409, "Employee.EmailNotUnique", $"The employee with email '{email}' already exist. ");
}

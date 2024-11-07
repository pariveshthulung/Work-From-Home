namespace Clean.Application.Wrappers;

public class UserErrors
{
    public static Error NotFound(Guid guidId) =>
        new(404, "User.NotFound", $"The user with Id = '{guidId}' was not found. ");

    public static Error NotFound() => new(404, "User.NotFound", $"The user  was not found. ");

    public static Error NotFound(int id) =>
        new(404, "User.NotFound", $"The user with Id = '{id}' was not found. ");

    public static Error NotFound(string email) =>
        new(404, "User.NotFound", $"The user with email '{email}' was not found. ");

    public static Error Unauthorize() => new(401, "User.Unauthorize", "The user is unauthorized.");

    public static Error Forbiden() =>
        new(403, "User.Forbiden", $"You don't have permission to access the resources. ");

    public static Error EmailNotUnique(string email) =>
        new(409, "User.EmailNotUnique", $"The user with email '{email}' already exist. ");
}

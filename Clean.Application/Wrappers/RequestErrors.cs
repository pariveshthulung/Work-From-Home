using System;

namespace Clean.Application.Wrappers;

public class RequestErrors
{
    public static Error NotFound(Guid guidId) =>
        new(404, "Request.NotFound", $"The request with Id = '{guidId}' was not found. ");

    public static Error NotFound() => new(404, "Request.NotFound", $"The request  was not found. ");

    public static Error NotFound(int id) =>
        new(404, "Request.NotFound", $"The request with Id = '{id}' was not found. ");

    public static Error NotFound(string email) =>
        new(404, "Request.NotFound", $"The request with email '{email}' was not found. ");

    public static Error PendingExist() =>
        new(
            401,
            "Request.PendingExist",
            "The previous request is on pending.Can't submitte a new request."
        );

    public static Error AreadyApproved() =>
        new(403, "Request.AlreadyApproved", $"Request has been already approved.");

    public static Error UnauthorizeToApprove() =>
        new(403, "Request.UnauthorizeToApprove", $"You don't have permission to approve request.");
}

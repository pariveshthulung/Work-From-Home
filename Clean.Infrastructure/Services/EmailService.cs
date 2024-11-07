using Clean.Application.Persistence.Contract;
using Clean.Application.Persistence.Services;
using Clean.Domain.Entities;
using Clean.Domain.Enums;
using FluentEmail.Core;

namespace Clean.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IEmployeeRepository _employeeRepo;

    public EmailService(IFluentEmail fluentEmail, IEmployeeRepository employeeRepository)
    {
        _fluentEmail = fluentEmail;
        _employeeRepo = employeeRepository;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        await _fluentEmail.To(toEmail).Subject(subject).Body(body).SendAsync();
    }

    public async Task SendRequestApprovedEmailAsync(
        Request request,
        CancellationToken cancellationToken
    )
    {
        var requestedBy = await _employeeRepo.GetEmployeeByIdAsync(
            request.RequestedBy,
            cancellationToken
        );
        var requestedTo = await _employeeRepo.GetEmployeeByIdAsync(
            request.RequestedTo,
            cancellationToken
        );
    }

    public async Task SendRequestSubmittedEmailAsync(
        Request request,
        CancellationToken cancellationToken
    )
    {
        var requestedBy = await _employeeRepo.GetEmployeeByIdAsync(
            request.RequestedBy,
            cancellationToken
        );
        var requestedTo = await _employeeRepo.GetEmployeeByIdAsync(
            request.RequestedTo,
            cancellationToken
        );
        var requestType = RequestTypeEnum.FromId(request.RequestedTypeId).Name;
        var body =
            $"Your request has been submitted from '{request.FromDate}' to {request.ToDate}'.";

        await _fluentEmail
            .To(requestedBy.Email)
            .Subject($"Request Submitted for '{requestType}'")
            .Body(body)
            .SendAsync();
        await _fluentEmail
            .To(requestedTo.Email)
            .Subject($"Request Submitted for '{requestType}'")
            .Body(body)
            .SendAsync();
    }
}

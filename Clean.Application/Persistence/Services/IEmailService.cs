using Clean.Domain.Entities;

namespace Clean.Application.Persistence.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendRequestApprovedEmailAsync(Request request, CancellationToken cancellationToken);
    Task SendRequestSubmittedEmailAsync(Request request, CancellationToken cancellationToken);
}

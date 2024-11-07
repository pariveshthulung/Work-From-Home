using System;
using Clean.Application.Persistence.Services;
using Clean.Domain.DomainEvents;
using MediatR;

namespace Clean.Application.Feature.Employees.Events;

public class RequestApprovedDomainEventHandler : INotificationHandler<RequestApprovedDomainEvent>
{
    private readonly IEmailService _emailService;

    public RequestApprovedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(
        RequestApprovedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        await _emailService.SendRequestApprovedEmailAsync(notification.Request, cancellationToken);
    }
}

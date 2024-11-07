using System;
using Clean.Domain.DomainEvents;
using MediatR;

namespace Clean.Application.Feature.Employees.Events;

public class RequestSubmittedDomainEventHandler : INotificationHandler<RequestApprovedDomainEvent>
{
    public Task Handle(RequestApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

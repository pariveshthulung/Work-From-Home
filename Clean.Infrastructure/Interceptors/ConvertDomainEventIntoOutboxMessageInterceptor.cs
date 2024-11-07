using System;
using Clean.Domain.Primitive;
using Clean.Infrastructure.OutBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Clean.Infrastructure.Interceptors;

public sealed class ConvertDomainEventIntoOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        var outBoxMessages = dbContext
            .ChangeTracker.Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvent = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvent();
                return domainEvent;
            })
            .Select(domainEvent => new OutBoxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                )
            })
            .ToList();
        dbContext.Set<OutBoxMessage>().AddRange(outBoxMessages);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

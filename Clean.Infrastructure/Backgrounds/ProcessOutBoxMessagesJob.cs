using System;
using System.Text.Json.Nodes;
using Azure.Core.Pipeline;
using Clean.Domain.Primitive;
using Clean.Infrastructure.Data;
using Clean.Infrastructure.OutBox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace Clean.Infrastructure.Backgrounds;

[DisallowConcurrentExecution]
public class ProcessOutBoxMessagesJob : IJob
{
    private readonly IPublisher _publisher;
    private readonly ApplicationDbContext _dbContext;

    public ProcessOutBoxMessagesJob(IPublisher publisher, ApplicationDbContext dbContext)
    {
        _publisher = publisher;
        _dbContext = dbContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutBoxMessage> messages = await _dbContext
            .OutBoxMessages.Where(x => x.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);
        foreach (OutBoxMessage outBoxMessage in messages)
        {
            IDomainEvent? domainEvent = null;

            try
            {
                domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outBoxMessage.Content,
                    new Newtonsoft.Json.JsonSerializerSettings
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    }
                );
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

            if (domainEvent is null)
            {
                //handle error
                //log here
                continue;
            }
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(50 * attempt));

            PolicyResult result = await retryPolicy.ExecuteAndCaptureAsync(
                () => _publisher.Publish(domainEvent, context.CancellationToken)
            );
            outBoxMessage.ProcessedOnUtc = DateTime.UtcNow;
            outBoxMessage.Errors = result.FinalException?.ToString();
        }
        await _dbContext.SaveChangesAsync();
    }
}

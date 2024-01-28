using GatherUs.Core.RabbitMq.Interfaces;

namespace GatherUs.API.HostedServices;

public class QueueMessageConsumerBackgroundServise : BackgroundService
{
    private readonly IMessageConsumer _messageConsumer;

    public QueueMessageConsumerBackgroundServise(IMessageConsumer messageConsumer)
    {
        _messageConsumer = messageConsumer;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageConsumer.ConsumeMessage();

        return Task.CompletedTask;    }
}
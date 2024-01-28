namespace GatherUs.Core.RabbitMq.Interfaces;

public interface IMessagePublisher
{
    void PublishMessage(QueueMessage message);
}
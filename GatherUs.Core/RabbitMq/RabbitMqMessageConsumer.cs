using System.Text;
using GatherUs.Core.RabbitMq.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace GatherUs.Core.RabbitMq;

public class RabbitMqMessageConsumer : RabbitMqConnector, IMessageConsumer
{
    public RabbitMqMessageConsumer()
    {
        EstablishConnection();
        DeclareWorkBrokerUnits();
    }

    public void ConsumeMessage()
    {
        if (_connection is null)
        {
            return;
        }

        ReopenChannelIfClosed();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += NotificationReceivedHandler;

        _channel.BasicConsume(
            queue: RabbitMqConstants.NotificationsQueue,
            autoAck: false,
            consumer: consumer);
    }

    protected async void NotificationReceivedHandler(object model, BasicDeliverEventArgs ea)
    {
        ReopenChannelIfClosed();

        try
        {
            var body = ea.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);

            var msg = GetMessage(messageString);

            RequeueMessage(ea, body);
        }
        finally
        {
            _channel.BasicAck(ea.DeliveryTag, false);
        }
    }

    private QueueMessage GetMessage(string jsonMessage)
    {
        var notification = JsonConvert.DeserializeObject<QueueMessage>(jsonMessage);
        return notification;
    }

    protected void RequeueMessage(BasicDeliverEventArgs ea, byte[] messageBody)
    {
        ReopenChannelIfClosed();

        long countOfRetry = 0;
        if (ea.BasicProperties.Headers is not null &&
            ea.BasicProperties.Headers.ContainsKey("x-death"))
        {
            var deathProperties = (List<object>)ea.BasicProperties.Headers["x-death"];
            var lastRetry = (Dictionary<string, object>)deathProperties[0];
            countOfRetry = (long)lastRetry["count"];
        }

        if (countOfRetry < RabbitMqConstants.MaxNumberOfRetrySending)
        {
            _channel.BasicPublish(
                RabbitMqConstants.RetryExchange,
                RabbitMqConstants.NotificationsQueue,
                ea.BasicProperties,
                messageBody);
        }
    }
}

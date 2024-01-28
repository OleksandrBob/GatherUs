using System.Text;
using GatherUs.Core.RabbitMq.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace GatherUs.Core.RabbitMq;

public class RabbitMqMessagePublisher : RabbitMqConnector, IMessagePublisher
{
    private readonly JsonSerializerSettings _serializationSettings;

    public RabbitMqMessagePublisher()
    {
        EstablishConnection();
        DeclareWorkBrokerUnits();
        DeclareRetryBrokerUnits();

        _serializationSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
        };
    }

    public void PublishMessage(QueueMessage message)
    {
        if (_channel is null)
        {
            return;
        }

        ReopenChannelIfClosed(true);

        var jsonData = JsonConvert.SerializeObject(message, _serializationSettings);
        var byteData = Encoding.UTF8.GetBytes(jsonData);

        var basicProperties = _channel.CreateBasicProperties();
        basicProperties.Persistent = true;

        _channel.BasicPublish(
            exchange: RabbitMqConstants.WorkExchange,
            routingKey: RabbitMqConstants.NotificationsQueue,
            basicProperties: basicProperties,
            body: byteData);
    }
}

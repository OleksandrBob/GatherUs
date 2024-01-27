using RabbitMQ.Client;

namespace GatherUs.Core.RabbitMq;

public abstract class RabbitMqConnector
{
    protected ConnectionFactory _connectionFactory;
    protected IConnection _connection;
    protected IModel _channel;

    protected virtual void EstablishConnection()
    {
        try
        {
            _connectionFactory = new ConnectionFactory { Uri = new Uri("amqp://localhost") };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        catch (Exception)
        {
            _connection = null;
            _channel = null;
            return;
        }
    }

    protected virtual void ReopenChannelIfClosed(bool? declareRetry = false)
    {
        if (_channel is null)
        {
            return;
        }

        if (_channel.IsClosed)
        {
            _channel = _connection.CreateModel();
            DeclareWorkBrokerUnits();
            if (declareRetry == true)
            {
                DeclareRetryBrokerUnits();
            }
        }
    }

    protected virtual void DeclareWorkBrokerUnits()
    {
        if (_channel is null)
        {
            return;
        }

        _channel.ExchangeDeclare(RabbitMqConstants.WorkExchange, RabbitMqConstants.DirectExchangeType);
        _channel.QueueDeclare(
            RabbitMqConstants.NotificationsQueue,
            true,
            false,
            false,
            null);

        _channel.QueueBind(
            RabbitMqConstants.NotificationsQueue,
            RabbitMqConstants.WorkExchange,
            RabbitMqConstants.NotificationsQueue,
            null);
    }

    protected virtual void DeclareRetryBrokerUnits()
    {
        if (_channel is null)
        {
            return;
        }

        var retryQueueArgs = new Dictionary<string, object>
        {
            { RabbitMqConstants.DeadLetterExchangeHeader, RabbitMqConstants.WorkExchange },
            { RabbitMqConstants.TTLHeader, RabbitMqConstants.MessageTTL },
        };

        _channel.ExchangeDeclare(RabbitMqConstants.RetryExchange, RabbitMqConstants.FanoutDirectExchangeType);
        
        _channel.QueueDeclare(
            RabbitMqConstants.RetryQueue,
            true,
            false,
            false,
            retryQueueArgs);
        
        _channel.QueueBind(
            RabbitMqConstants.RetryQueue,
            RabbitMqConstants.RetryExchange,
            string.Empty,
            null);
    }
}

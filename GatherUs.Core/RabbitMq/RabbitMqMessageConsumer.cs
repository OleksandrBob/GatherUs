using System.Text;
using GatherUs.Core.Mailing;
using GatherUs.Core.Mailing.SetUp;
using GatherUs.Core.RabbitMq.Interfaces;
using GatherUs.DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Organizer = GatherUs.DAL.Models.Organizer;

namespace GatherUs.Core.RabbitMq;

public class RabbitMqMessageConsumer : RabbitMqConnector, IMessageConsumer
{
    private readonly IMailingService _mailingService;

    public RabbitMqMessageConsumer(IMailingService mailingService)
    {
        _mailingService = mailingService;
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

    private async void NotificationReceivedHandler(object model, BasicDeliverEventArgs ea)
    {
        ReopenChannelIfClosed();
        var body = ea.Body.ToArray();

        try
        {
            var messageString = Encoding.UTF8.GetString(body);

            var msg = GetMessage(messageString);
            await SendMessage(msg);
        }
        catch
        {
            RequeueMessage(ea, body);
        }
        finally
        {
            _channel.BasicAck(ea.DeliveryTag, false);
        }
    }

    private async Task SendMessage(QueueMessage message)
    {
        switch (message.Type)
        {
            case MailType.ConfirmationCode:
                await _mailingService.SendMailVerificationCodeAsync((message.MessageValue as JObject)
                    .ToObject<EmailForRegistration>());
                break;
            case MailType.GuestVerification:
                await _mailingService.SendGuestVerificationMailAsync(
                    (message.MessageValue as JObject).ToObject<Guest>());
                break;
            case MailType.OrganizerVerification:
                await _mailingService.SendOrganizerVerificationMailAsync((message.MessageValue as JObject)
                    .ToObject<Organizer>());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private QueueMessage GetMessage(string jsonMessage)
    {
        var notification = JsonConvert.DeserializeObject<QueueMessage>(jsonMessage);
        return notification;
    }

    private void RequeueMessage(BasicDeliverEventArgs ea, byte[] messageBody)
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

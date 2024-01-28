using GatherUs.Core.Mailing.SetUp;

namespace GatherUs.Core.RabbitMq;

public class QueueMessage
{
    public MailType Type { get; set; }

    public object MessageValue { get; set; }
}
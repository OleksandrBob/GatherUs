using GatherUs.Core.Mailing.SetUp;

namespace GatherUs.Core.RabbitMq;

public class QueueMessage
{
    public MailType Type { get; init; }

    public object MessageValue { get; init; }
}

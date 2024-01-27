namespace GatherUs.Core.RabbitMq;

public static class RabbitMqConstants
{
    public static string WorkExchange => "WORK_EXCHANGE";

    public static string RetryExchange => "RETRY_EXCHANGE";

    public static string RetryQueue => "RETRY_QUEUE";

    public static string NotificationsQueue => "MAIL_QUEUE";

    public static string DirectExchangeType => "direct";

    public static string FanoutDirectExchangeType => "fanout";

    public static string DeadLetterExchangeHeader => "x-dead-letter-exchange";

    public static string TTLHeader => "x-message-ttl";

    public static int MessageTTL => 30_000;

    public static int MaxNumberOfRetrySending => 5;
}

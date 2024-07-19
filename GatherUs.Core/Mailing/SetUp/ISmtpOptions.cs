namespace GatherUs.Core.Mailing.SetUp;

public interface ISmtpOptions
{
    public string Port { get; }
    
    public string Host { get; }

    public string UserName { get; }

    public string Password { get; }
}
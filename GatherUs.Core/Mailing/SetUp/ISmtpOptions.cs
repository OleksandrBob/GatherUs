namespace GatherUs.Core.Mailing.SetUp;

public interface ISmtpOptions
{
    public int Port { get; }
    
    public string Host { get; }

    public string UserName { get; }

    public string Password { get; }
}
namespace GatherUs.Core.Mailing.SetUp;

public class SmtpOptions : ISmtpOptions
{
    public string Host => HostConfig;

    public string HostConfig { get; set; }
    
    public int Port => PortConfig;

    public int PortConfig { get; set; }
    
    public string UserName => UserNameConfig;

    public string UserNameConfig { get; set; }
    
    public string Password => PasswordConfig;

    public string PasswordConfig { get; set; }
}
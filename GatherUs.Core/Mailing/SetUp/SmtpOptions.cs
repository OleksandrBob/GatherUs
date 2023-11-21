namespace GatherUs.Core.Mailing.SetUp;

public class SmtpOptions : ISmtpOptions
{
    public string Host => HostConfig;

    public string HostConfig { get; set; }

    //public string HostEnvironment { get; set; } = EnvironmentVariablesProvider.SmtpHost;

    public int Port => PortConfig;

    public int PortConfig { get; set; }

    //public int? PortEnvironment { get; set; } = EnvironmentVariablesProvider.SmtpPort;

    public string UserName => UserNameConfig;

    public string UserNameConfig { get; set; }

    //public string UserNameEnvironment { get; set; } = EnvironmentVariablesProvider.SmtpUsername;

    public string Password => PasswordConfig;

    public string PasswordConfig { get; set; }

    //public string PasswordEnvironment { get; set; } = EnvironmentVariablesProvider.SmtpPassword;
}
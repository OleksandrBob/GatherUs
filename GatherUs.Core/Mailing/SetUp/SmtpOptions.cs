using GatherUs.Core.Helpers;

namespace GatherUs.Core.Mailing.SetUp;

public class SmtpOptions : ISmtpOptions
{
    public string HostEnvironment => EnvironmentVariablesHelper.SmtpHost;

    public string HostConfig { get; set; }

    public string Host => HostEnvironment ?? HostConfig;

    public string PortEnvironment => EnvironmentVariablesHelper.SmtpPort;

    public string PortConfig { get; set; }

    public string Port => PortEnvironment ?? PortConfig;

    public string UserNameEnvironment => EnvironmentVariablesHelper.SmtpUserName;

    public string UserNameConfig { get; set; }

    public string UserName => UserNameEnvironment ?? UserNameConfig;

    public string PasswordEnvironment => EnvironmentVariablesHelper.SmtpPassword;

    public string PasswordConfig { get; set; }

    public string Password => PasswordEnvironment ?? PasswordConfig;
}
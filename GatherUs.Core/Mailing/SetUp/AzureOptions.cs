using GatherUs.Core.Helpers;

namespace GatherUs.Core.Mailing.SetUp;

public class AzureOptions : IAzureOptions
{
    public string ConnectionStringEnvironment => EnvironmentVariablesHelper.AzureConnectionString;
    
    public string ConnectionStringConfig { get; set; }

    public string ConnectionString => ConnectionStringEnvironment ?? ConnectionStringConfig;
}

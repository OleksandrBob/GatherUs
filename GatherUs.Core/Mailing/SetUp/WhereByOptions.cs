using GatherUs.Core.Helpers;

namespace GatherUs.Core.Mailing.SetUp;

public class WhereByOptions : IWhereByOptions
{
    public string ApiKeyEnvironment => EnvironmentVariablesHelper.WhereByKey;
    
    public string ApiKeyConfig { get; set; }

    public string ApiKey => ApiKeyEnvironment ?? ApiKeyConfig;

    public string ApiUrlEnvironment => EnvironmentVariablesHelper.WhereByUrl;
    
    public string ApiUrlConfig { get; set; }

    public string ApiUrl => ApiUrlEnvironment ?? ApiUrlConfig;
}

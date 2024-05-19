namespace GatherUs.Core.Mailing.SetUp;

public class WhereByOptions : IWhereByOptions
{
    public string ApiKeyConfig { get; set; }

    public string ApiKey => ApiKeyConfig;

    public string ApiUrlConfig { get; set; }

    public string ApiUrl => ApiUrlConfig;
}

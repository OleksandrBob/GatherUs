namespace GatherUs.Core.Mailing.SetUp;

public class AzureOptions : IAzureOptions
{
    public string ConnectionStringConfig { get; set; }

    public string ConnectionString => ConnectionStringConfig;
}

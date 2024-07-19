using GatherUs.Core.Helpers;

namespace GatherUs.Core.Mailing.SetUp;

public class BraintreeOptions : IBraintreeOptions
{
    public string MerchantIdEnvironment => EnvironmentVariablesHelper.BraintreeMerchantId;

    public string MerchantIdConfig { get; set; }

    public string MerchantId => MerchantIdEnvironment ?? MerchantIdConfig;

    public string PublicKeyEnvironment => EnvironmentVariablesHelper.BraintreePublicKey;

    public string PublicKeyConfig { get; set; }

    public string PublicKey => PublicKeyEnvironment ?? PublicKeyConfig;

    public string PrivateKeyEnvironment => EnvironmentVariablesHelper.BraintreePrivateKey;

    public string PrivateKeyConfig { get; set; }

    public string PrivateKey => PrivateKeyEnvironment ?? PrivateKeyConfig;
}

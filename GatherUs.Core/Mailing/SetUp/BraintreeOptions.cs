namespace GatherUs.Core.Mailing.SetUp;

public class BraintreeOptions : IBraintreeOptions
{
    public string MerchantIdConfig { get; set; }
    
    public string MerchantId => MerchantIdConfig;
    
    public string PublicKeyConfig { get; set; }
    
    public string PublicKey => PublicKeyConfig;
    
    public string PrivateKeyConfig { get; set; }
    
    public string PrivateKey => PrivateKeyConfig;
}

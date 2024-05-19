namespace GatherUs.Core.Mailing.SetUp;

public interface IBraintreeOptions
{
    public string MerchantId { get; }

    public string PublicKey { get; }

    public string PrivateKey { get; }
}

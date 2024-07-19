namespace GatherUs.Core.Constants;

public static class AppConstants
{
    public static string Salt => "VeniVidiViciVeniVidiViciVeniVidiViciVeniVidiViciVeniVidiVici";

    public static string JwtAudience => "GatherUsClient";

    public static string JwtIssuer => "GatherUsServer";

    public const string OrganizerRole = "Organizer";

    public const string GuestRole =  "Guest";
    
    public const string BearerAuth  = "bearerAuth";
    
    public const string SmtpHost = "SMTP_HOST";

    public const string SmtpPort = "SMTP_PORT";

    public const string SmtpUserName = "SMTP_USERNAME";

    public const string SmtpPassword = "SMTP_PASSWORD";

    public const string BraintreeMerchantId = "BRAINTREE_MERCHANT_ID";
    
    public const string BraintreePublicKey = "BRAINTREE_PYBLIC_KEY";
    
    public const string BraintreePrivateKey = "BRAINTREE_PRIVATE_KEY";
    
    public const string AzureConnectionString = "AZURE_CONNECTION_STRING";
    
    public const string WhereByUrl = "WHERE_BY_URL";
    
    public const string WhereByKey = "WHERE_BY_KEY";
}

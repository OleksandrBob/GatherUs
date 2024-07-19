using GatherUs.Core.Constants;

namespace GatherUs.Core.Helpers;

public static class EnvironmentVariablesHelper
{
    public static string SmtpHost => Environment.GetEnvironmentVariable(AppConstants.SmtpHost);
    
    public static string SmtpPort => Environment.GetEnvironmentVariable(AppConstants.SmtpPort);
    
    public static string SmtpUserName => Environment.GetEnvironmentVariable(AppConstants.SmtpUserName);
    
    public static string SmtpPassword => Environment.GetEnvironmentVariable(AppConstants.SmtpPassword);
    
    public static string WhereByKey => Environment.GetEnvironmentVariable(AppConstants.WhereByKey);
    
    public static string WhereByUrl => Environment.GetEnvironmentVariable(AppConstants.WhereByUrl);
    
    public static string BraintreeMerchantId => Environment.GetEnvironmentVariable(AppConstants.BraintreeMerchantId);
    
    public static string BraintreePublicKey => Environment.GetEnvironmentVariable(AppConstants.BraintreePublicKey);
    
    public static string BraintreePrivateKey => Environment.GetEnvironmentVariable(AppConstants.BraintreePrivateKey);
    
    public static string AzureConnectionString => Environment.GetEnvironmentVariable(AppConstants.AzureConnectionString);
}

using System.Security.Cryptography;
using System.Text;
using GatherUs.Core.Constants;

namespace GatherUs.Core.Helpers;

public static class CryptoHelper
{
    public static string GenerateSaltedHash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var saltBytes = Encoding.ASCII.GetBytes(AppConstants.Salt);

        Rfc2898DeriveBytes rfc2898DeriveBytes = new(password, saltBytes, 10000);
        var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

        return hashPassword;
    }
}
using GatherUs.DAL.Models;

namespace GatherUs.Core.Services.Interfaces;

public interface ITokenManager
{
    string GenerateJwt(User user);
}

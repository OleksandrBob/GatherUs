using GatherUs.DAL.Models;

namespace GatherUs.Core.Mailing.Dto;

public class UserForMailDto
{
    public readonly User User;

    public UserForMailDto(User user)
    {
        User = user;
    }
}
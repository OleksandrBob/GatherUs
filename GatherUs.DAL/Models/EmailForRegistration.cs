using GatherUs.Enums;

namespace GatherUs.DAL.Models;

public class EmailForRegistration : EntityBase
{
    public string Email { get; set; }

    public ushort ConfirmationCode { get; set; }

    public UserType UserType { get; set; }
}
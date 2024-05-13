using GatherUs.Enums;

namespace GatherUs.DAL.Models;

public abstract class User : EntityBase
{
    public UserType UserType { get; set; }

    public string Mail { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }
    
    public decimal Balance { get; set; }
    
    public string BrainTreeId { get; set; }

    public string ProfilePictureUrl { get; set; }
}
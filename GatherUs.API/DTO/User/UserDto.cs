namespace GatherUs.API.DTO.User;

public class UserDto
{
    public string Mail { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public decimal Balance { get; set; }

    public string ProfilePictureUrl { get; set; }
}
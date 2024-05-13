using GatherUs.Enums;

namespace GatherUs.API.DTO.Guest;

public class GuestDto
{
    public UserType UserType { get; set; }

    public string Mail { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
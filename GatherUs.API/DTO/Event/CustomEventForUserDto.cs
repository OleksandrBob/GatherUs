using GatherUs.Enums.DAL;

namespace GatherUs.API.DTO.Event;

public class CustomEventForUserDto : CustomEventDto
{
    public UserType UserType { get; set; }

    public bool IsUserAttending { get; set; }
}
using GatherUs.API.DTO.User;
using GatherUs.Enums.DAL;

namespace GatherUs.API.DTO.Event;

public class AttendanceInviteDto
{
    public int Id { get; set; }
    
    public int CustomEventId { get; set; }
    
    public string Message { get; set; }

    public UserDto UserDto { get; set; }

    public SmallEventDto SmallEventDto { get; set; }

    public InviteStatus InviteStatus { get; set; }
}
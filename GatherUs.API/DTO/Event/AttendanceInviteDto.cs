using GatherUs.Enums.DAL;

namespace GatherUs.API.DTO.Event;

public class AttendanceInviteDto
{
    public int CustomEventId { get; set; }

    public InviteStatus InviteStatus { get; set; }
}
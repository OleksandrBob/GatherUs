using GatherUs.Enums.DAL;

namespace GatherUs.DAL.Models;

public class AttendanceInvite : EntityBase
{
    public int CustomEventId { get; set; }

    public CustomEvent CustomEvent { get; set; }

    public int GuestId { get; set; }

    public Guest Guest { get; set; }

    public string Message { get; set; }

    public InviteStatus InviteStatus { get; set; }
}

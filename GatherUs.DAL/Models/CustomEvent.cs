using GatherUs.Enums.DAL;

namespace GatherUs.DAL.Models;

public class CustomEvent : EntityBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int OrganizerId { get; set; }

    public Organizer Organizer { get; set; }

    public List<Guest> Attendants { get; set; }

    public List<AttendanceInvite> Invites { get; set; }

    public DateTime StartTimeUtc { get; set; }

    public byte MinRequiredAge { get; set; }

    public decimal TicketPrice { get; set; }

    public CustomEventType CustomEventType { get; set; }

    public CustomEventLocationType CustomEventLocationType { get; set; }

    public CustomEventCategory CustomEventCategory { get; set; }
}

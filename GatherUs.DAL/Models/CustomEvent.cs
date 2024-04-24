using GatherUs.Enums.DAL;

namespace GatherUs.DAL.Models;

public class CustomEvent : EntityBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int OrganizerId { get; set; }

    public Organizer Organizer { get; set; }

    public List<Guest> Attendants { get; set; }

    public List<CustomEventGuest> CustomEventGuests { get; set; }

    public List<AttendanceInvite> Invites { get; set; }

    public DateTime StartTimeUtc { get; set; }

    public byte MinRequiredAge { get; set; }

    public decimal TicketPrice { get; set; }
    
    public string PictureUrl { get; set; }
    
    public int TotalTicketCount { get; set; }
    
    public int TicketsLeft { get; set; }

    public uint? MeetingId { get; set; }
    
    public string RoomUrl { get; set; }

    public string HostRoomUrl { get; set; }

    public string Location { get; set; }

    public double? LocationLatitude { get; set; }
    
    public double? LocationLongitude { get; set; }
    
    public CustomEventType CustomEventType { get; set; }

    public CustomEventLocationType CustomEventLocationType { get; set; }
    
    public List<CustomEventCategory> CustomEventCategories { get; set; }
}

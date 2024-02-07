namespace GatherUs.DAL.Models;

public class Guest : User
{
    public List<CustomEvent> PaidEvents { get; set; }
    
    public List<CustomEventGuest> CustomEventGuests { get; set; }
    
    public List<AttendanceInvite> Invites { get; set; }
}
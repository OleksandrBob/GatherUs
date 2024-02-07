namespace GatherUs.DAL.Models;

public class CustomEventGuest
{
    public int CustomEventId { get; set; }

    public CustomEvent CustomEvent { get; set; }

    public int GuestId { get; set; }

    public Guest Guest { get; set; }
}

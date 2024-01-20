namespace GatherUs.DAL.Models;

public class Organizer : User
{
    public List<CustomEvent> CreatedEvents { get; set; }
}
using GatherUs.Enums.DAL;

namespace GatherUs.API.DTO.Event;

public class CustomEventDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string PictureUrl { get; set; }
    
    public string Description { get; set; }

    public DateTime StartTimeUtc { get; set; }

    public byte MinRequiredAge { get; set; }

    public decimal TicketPrice { get; set; }
    
    public int TotalTicketCount { get; set; }
    
    public int TicketsLeft { get; set; }

    public string RoomUrl { get; set; }
    
    public CustomEventType CustomEventType { get; set; }

    public CustomEventLocationType CustomEventLocationType { get; set; }

    public List<CustomEventCategory> CustomEventCategories { get; set; }
}

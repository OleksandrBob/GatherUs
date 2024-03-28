namespace GatherUs.API.DTO.Event;

public class SmallEventDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string PictureUrl { get; set; }
    
    public DateTime StartTimeUtc { get; set; }
}
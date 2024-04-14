namespace GatherUs.DAL.Models;

public class GatherUsPaymentTransaction : EntityBase
{
    public int GuestId { get; set; }

    public Guest Guest { get; set; }

    public int OrganizerId { get; set; }

    public Organizer Organizer { get; set; }

    public int CustomEventId { get; set; }

    public CustomEvent CustomEvent { get; set; }

    public decimal TransactionAmount { get; set; }

    public decimal Fee { get; set; }
}

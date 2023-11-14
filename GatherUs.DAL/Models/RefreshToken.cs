namespace GatherUs.DAL.Models;

public class RefreshToken : EntityBase
{
    public User User { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Revoked { get; set; }

    public string ReplacedByToken { get; set; }
}
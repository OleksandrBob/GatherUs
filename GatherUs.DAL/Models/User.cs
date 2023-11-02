using GatherUs.DAL.Enums;

namespace GatherUs.DAL.Models;

public class User
{
    public UserType UserType { get; set; }
    public DateTime? DeletionTime { get; set; }
}
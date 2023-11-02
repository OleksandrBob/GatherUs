using GatherUs.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GatherUs.DAL.Context;

public interface IDataContext
{
    public DbSet<Guest> Guests { get; set; }
    
    public DbSet<Organizer> Organizers { get; set; }
}
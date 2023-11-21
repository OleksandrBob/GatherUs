using GatherUs.DAL.Models;

namespace GatherUs.Core.Services.Interfaces;

public interface IOrganizerService
{
    Task InsertAsync(Organizer guestToAdd);
}
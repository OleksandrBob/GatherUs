using GatherUs.DAL.Models;

namespace GatherUs.Core.Services.Interfaces;

public interface IGuestService
{
    Task InsertAsync(Guest guestToAdd);
}
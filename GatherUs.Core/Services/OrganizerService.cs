using GatherUs.Core.Helpers;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;

namespace GatherUs.Core.Services;

public class OrganizerService : IOrganizerService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrganizerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task InsertAsync(Organizer organizerToAdd)
    {
        organizerToAdd.Password = CryptoHelper.GenerateSaltedHash(organizerToAdd.Password);
        
        _unitOfWork.Organizers.AddNew(organizerToAdd);
        await _unitOfWork.CompleteAsync();
    }
}
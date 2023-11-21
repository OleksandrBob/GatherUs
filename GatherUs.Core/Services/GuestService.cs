using GatherUs.Core.Helpers;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;

namespace GatherUs.Core.Services;

public class GuestService : IGuestService
{
    private readonly IUnitOfWork _unitOfWork;

    public GuestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task InsertAsync(Guest guestToAdd)
    {
        guestToAdd.Password = CryptoHelper.GenerateSaltedHash(guestToAdd.Password);
        
        _unitOfWork.Guests.AddNew(guestToAdd);
        await _unitOfWork.CompleteAsync();
    }
}
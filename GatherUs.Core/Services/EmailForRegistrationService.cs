using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums.DAL;

namespace GatherUs.Core.Services;

public class EmailForRegistrationService : IEmailForRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmailForRegistrationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EmailForRegistration> GetEmailForRegistrationAsync(string email)
    {
        return await _unitOfWork.EmailForRegistrations.GetFirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Result> AddEmailForRegistration(string email, UserType userType)
    {
        var alreadyExistMail = await _unitOfWork.EmailForRegistrations.AnyAsync(e => e.Email == email);
        if (alreadyExistMail)
        {
            return Result.Failure("This mail has already been used for registration process.");
        }

        var alreadyExistUser = await _unitOfWork.Users.AnyAsync(e => e.Mail == email);
        if (alreadyExistUser)
        {
            return Result.Failure("User with this mail already exist.");
        }

        Random rnd = new();
        _unitOfWork.EmailForRegistrations.AddNew(new EmailForRegistration
            { Email = email, UserType = userType, ConfirmationCode = (ushort)rnd.Next(1111, 9999) });
        
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }
}

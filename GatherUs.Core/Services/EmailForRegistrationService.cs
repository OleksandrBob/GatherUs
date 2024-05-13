using CSharpFunctionalExtensions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums;

namespace GatherUs.Core.Services;

public class 
    EmailForRegistrationService : IEmailForRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmailForRegistrationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task RemoveEmailForRegistrationAsync(int emailId)
    {
        await _unitOfWork.EmailForRegistrations.Remove(emailId);
        await _unitOfWork.CompleteAsync();
    }    

    public async Task<EmailForRegistration> GetEmailForRegistrationAsync(string email)
    {
        return await _unitOfWork.EmailForRegistrations.GetFirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Result<EmailForRegistration>> AddEmailForRegistration(string email, UserType userType)
    {
        var alreadyExistMail = await _unitOfWork.EmailForRegistrations.AnyAsync(e => e.Email == email);
        if (alreadyExistMail)
        {
            return Result.Failure<EmailForRegistration>("This mail has already been used for registration process.");
        }

        var alreadyExistUser = await _unitOfWork.Users.AnyAsync(e => e.Mail == email);
        if (alreadyExistUser)
        {
            return Result.Failure<EmailForRegistration>("User with this mail already exist.");
        }

        Random rnd = new();
        var mailToAdd = new EmailForRegistration
            { Email = email, UserType = userType, ConfirmationCode = (ushort)rnd.Next(1111, 9999) };

        _unitOfWork.EmailForRegistrations.AddNew(mailToAdd);

        await _unitOfWork.CompleteAsync();

        return Result.Success(mailToAdd);
    }
}

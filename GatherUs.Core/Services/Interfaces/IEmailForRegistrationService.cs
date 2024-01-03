using CSharpFunctionalExtensions;
using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;

namespace GatherUs.Core.Services.Interfaces;

public interface IEmailForRegistrationService
{
    Task RemoveEmailForRegistrationAsync(int emailId);
    
    Task<EmailForRegistration> GetEmailForRegistrationAsync(string email);

    Task<Result<EmailForRegistration>> AddEmailForRegistration(string email, UserType userType);
}
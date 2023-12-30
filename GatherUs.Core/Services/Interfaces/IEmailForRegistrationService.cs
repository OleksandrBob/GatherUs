using CSharpFunctionalExtensions;
using GatherUs.DAL.Models;
using GatherUs.Enums.DAL;

namespace GatherUs.Core.Services.Interfaces;

public interface IEmailForRegistrationService
{
    Task<EmailForRegistration> GetEmailForRegistrationAsync(string email);

    Task<Result> AddEmailForRegistration(string email, UserType userType);
}
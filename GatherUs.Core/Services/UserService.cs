using System.Linq.Expressions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using GatherUs.Core.Helpers;
using GatherUs.Core.Mailing.SetUp;

namespace GatherUs.Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BlobContainerClient _imagesContainerClient;

    private const string ContainerName = "images";

    public UserService(IUnitOfWork unitOfWork, IAzureOptions azureOptions)
    {
        _unitOfWork = unitOfWork;

        var blobServiceClient = new BlobServiceClient(azureOptions.ConnectionString);
        _imagesContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
    }

    public async Task<User> GetByEmailAsync(string email, params Expression<Func<User, object>>[] includes)
        => await _unitOfWork.Users.GetByAsync(c => c.Mail == email, includes: includes);

    public async Task<User> GetByIdAsync(int id, params Expression<Func<User, object>>[] includes)
        => await _unitOfWork.Users.GetByAsync(c => c.Id == id, includes: includes);

    public async Task<Result> DeleteProfilePicture(int userId)
    {
        var user = await _unitOfWork.Users.GetByAsync(u => u.Id == userId);
        if (user is null)
        {
            return Result.Failure("User with provided Id was not found");
        }

        if (user.ProfilePictureUrl is not null)
        {
            var blobClient =
                _imagesContainerClient.GetBlobClient(
                    user.ProfilePictureUrl.Replace("https://gatherus.blob.core.windows.net/images/", ""));
            await blobClient.DeleteAsync();
        }

        user.ProfilePictureUrl = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result<string>> UploadProfilePicture(int userId, string imageFile, string name)
    {
        try
        {
            if (string.IsNullOrEmpty(imageFile))
            {
                return Result.Failure<string>("File is empty");
            }

            var ext = name.Split('.')[1];
            var fileName = PictureHelper.GetUserProfilePictureUrl(userId) + '.' + ext;
            var imageBytes = Convert.FromBase64String(imageFile);

            using var memoryStream = new MemoryStream(imageBytes);
            var blobClient = _imagesContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(memoryStream, overwrite: true);

            var user = await _unitOfWork.Users.GetByAsync(u => u.Id == userId);
            user.ProfilePictureUrl = "https://gatherus.blob.core.windows.net/images/" + fileName;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return user.ProfilePictureUrl;
        }
        catch
        {
            return Result.Failure<string>("Saving failed.");
        }
    }

    public async Task SetBrainTreeId(string email, string brainTreeId)
    {
        var user = await _unitOfWork.Users.GetByAsync(u => u.Mail == email);
        user.BrainTreeId = brainTreeId;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task AddMoney(int userId, decimal amount)
    {
        var user = await _unitOfWork.Users.GetByAsync(u => u.Id == userId);
        user.Balance += amount;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();
    }
}

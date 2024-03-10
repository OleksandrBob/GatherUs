using System.Linq.Expressions;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace GatherUs.Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BlobClient _imagesContainerClient;

    //TODO: Remove strings from here
    private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=gatherus;AccountKey=L7c5tB9b2UDkYeURe0jL+35lgAPSEwkTq5cwubkmM5kGl+JJeJR062fnOJ7syn3S/sJBLjblSDkq+AStq/Ubcw==;EndpointSuffix=core.windows.net";
    private const string ContainerName = "images";
    
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        var blobServiceClient = new BlobServiceClient(ConnectionString);;
        _imagesContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName).GetBlobClient(ContainerName);
    }

    public async Task<User> GetByEmailAsync(string email, params Expression<Func<User, object>>[] includes)
        => await _unitOfWork.Users.GetByAsync(c => c.Mail == email, includes: includes);

    public async Task<User> GetByIdAsync(int id, params Expression<Func<User, object>>[] includes)
        => await _unitOfWork.Users.GetByAsync(c => c.Id == id, includes: includes);

    public async Task DeleteProfilePicture(int userId)
    {
        var user = await _unitOfWork.Users.GetByAsync(u => u.Id == userId);
        if (user is null)
        {
            return;
        }

        user.ProfilePictureUrl = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UploadProfilePicture(int userId)
    {
        //await _imagesContainerClient.UploadAsync(null, overwrite: true);
    }
}

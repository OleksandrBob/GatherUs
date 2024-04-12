using System.Net.Http.Headers;
using System.Text;
using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using GatherUs.Core.ApiObjects;
using GatherUs.Core.Helpers;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using GatherUs.DAL.Repository;
using GatherUs.Enums.DAL;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GatherUs.Core.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BlobContainerClient _imagesContainerClient;
    private readonly IHttpClientFactory _httpClientFactory;

    //TODO: Remove strings from here
    private const string ConnectionString =
        "DefaultEndpointsProtocol=https;AccountName=gatherus;AccountKey=L7c5tB9b2UDkYeURe0jL+35lgAPSEwkTq5cwubkmM5kGl+JJeJR062fnOJ7syn3S/sJBLjblSDkq+AStq/Ubcw==;EndpointSuffix=core.windows.net";

    private const string WhereByKey =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmFwcGVhci5pbiIsImF1ZCI6Imh0dHBzOi8vYXBpLmFwcGVhci5pbi92MSIsImV4cCI6OTAwNzE5OTI1NDc0MDk5MSwiaWF0IjoxNzExNDcxNjY1LCJvcmdhbml6YXRpb25JZCI6MjIxMTA4LCJqdGkiOiIzOGQyZTlhZS1jNjk1LTQxMTMtOGVkZi1jNWEzZDIxNDk0NzMifQ._wOeA5JR6WKPlKCUqu158QSlJ17grDVAjbqMlaqwPAc";

    private const string ContainerName = "images";

    public EventService(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
    {
        _unitOfWork = unitOfWork;
        _httpClientFactory = httpClientFactory;

        var blobServiceClient = new BlobServiceClient(ConnectionString);
        _imagesContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
    }

    public async Task<int>
        CreateEvent(
            int organizerId,
            string name,
            string description,
            DateTime startTimeUtc,
            byte minRequiredAge,
            decimal ticketPrice,
            int ticketCount,
            string image,
            string imageName,
            CustomEventType customEventType,
            CustomEventLocationType customEventLocationType,
            List<CustomEventCategory> customEventCategories)
    {
        uint? meetingId = null;
        string roomUrl = null;
        string hostRoomUrl = null;

        if (customEventLocationType == CustomEventLocationType.Online)
        {
            var stringPayload = JsonConvert.SerializeObject(new
            {
                roomMode = "group", isLocked = false, fields = new[] { "hostRoomUrl" },
                endDate = startTimeUtc.AddDays(2)
            });
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WhereByKey);

            var createdMeetingResponse = await client.PostAsync("https://api.whereby.dev/v1/meetings", httpContent);
            var responseBody = await createdMeetingResponse.Content.ReadAsStringAsync();

            var createdMeeting = JsonConvert.DeserializeObject<CreateMeetingResponse>(responseBody);

            if (createdMeeting is not null)
            {
                roomUrl = createdMeeting.RoomUrl;
                meetingId = Convert.ToUInt32(createdMeeting.MeetingId);
                hostRoomUrl = createdMeeting.HostRoomUrl;
            }
        }

        var eventToCreate = new CustomEvent
        {
            Name = name,
            RoomUrl = roomUrl,
            MeetingId = meetingId,
            HostRoomUrl = hostRoomUrl,
            OrganizerId = organizerId,
            Description = description,
            TicketPrice = ticketPrice,
            TicketsLeft = ticketCount,
            StartTimeUtc = startTimeUtc,
            TotalTicketCount = ticketCount,
            MinRequiredAge = minRequiredAge,
            CustomEventType = customEventType,
            CustomEventCategories = customEventCategories,
            CustomEventLocationType = customEventLocationType,
        };

        _unitOfWork.CustomEvents.AddNew(eventToCreate);
        await _unitOfWork.CompleteAsync();

        if (string.IsNullOrEmpty(image) || string.IsNullOrEmpty(imageName))
        {
            return eventToCreate.Id;
        }

        var ext = imageName.Split('.')[1];
        var fileName = PictureHelper.GetEventPictureUrl(eventToCreate.Id) + '.' + ext;
        var imageBytes = Convert.FromBase64String(image);

        using var memoryStream = new MemoryStream(imageBytes);
        var blobClient = _imagesContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(memoryStream, overwrite: true);

        eventToCreate.PictureUrl = "https://gatherus.blob.core.windows.net/images/" + fileName;
        await _unitOfWork.CompleteAsync();

        return eventToCreate.Id;
    }

    public async Task<List<CustomEvent>> GetEventsByUserId(int userId, bool showPastEvents = false)
    {
        if (showPastEvents)
        {
            return await _unitOfWork.CustomEvents.GetAllAsync(e => e.StartTimeUtc < DateTime.UtcNow &&
                                                                   (e.OrganizerId == userId ||
                                                                    e.Attendants.Any(a => a.Id == userId)));
        }

        return await _unitOfWork.CustomEvents.GetAllAsync(e => e.StartTimeUtc >= DateTime.UtcNow &&
                                                               (e.OrganizerId == userId ||
                                                                e.Attendants.Any(a => a.Id == userId)));
    }

    public async Task<CustomEvent> GetEventById(int eventId)
    {
        return await _unitOfWork.CustomEvents.GetFirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<AttendanceInvite> InviteUser(int guestId, int customEventId, string inviteMessage)
    {
        AttendanceInvite invite = new()
        {
            GuestId = guestId,
            Message = inviteMessage,
            CustomEventId = customEventId,
            InviteStatus = InviteStatus.Pending,
        };

        _unitOfWork.AttendanceInvites.AddNew(invite);
        await _unitOfWork.CompleteAsync();

        return invite;
    }

    public async Task<List<AttendanceInvite>> GetGuestInvites(int guestId, InviteStatus inviteStatus)
    {
        return await _unitOfWork.AttendanceInvites.GetAllAsync(
            predicate: e => e.GuestId == guestId && e.InviteStatus == inviteStatus,
            include: i => i.Include(inv => inv.CustomEvent)
                .ThenInclude(inv => inv.Organizer));
    }

    public async Task<List<AttendanceInvite>> GetSentInvites(int organizerId,
        InviteStatus inviteStatus = InviteStatus.Pending)
    {
        return await _unitOfWork.AttendanceInvites.GetAllAsync(
            predicate: e => e.CustomEvent.OrganizerId == organizerId && e.InviteStatus == inviteStatus,
            include: i => i.Include(inv => inv.CustomEvent)
                .Include(inv => inv.Guest));
    }

    public async Task<List<Guest>> GetGuestsInvitedToEvent(int customEventId)
    {
        return await _unitOfWork.AttendanceInvites.GetSelectAsync(
            selector: e => e.Guest,
            predicate: e => e.CustomEventId == customEventId,
            include: i => i.Include(ee => ee.Guest));
    }

    public async Task<List<CustomEvent>> GetFilteredEvents(
        string searchString,
        DateTime? fromDate,
        DateTime? toDate,
        byte? fromMinRequiredAge,
        byte? toMinRequiredAge,
        decimal? fromTicketPrice,
        decimal? toTicketPrice,
        List<CustomEventType> customEventTypes,
        List<CustomEventLocationType> customEventLocationTypes,
        List<CustomEventCategory> customEventCategories)
    {
        var predicate = PredicateBuilder.New<CustomEvent>(c => c.CustomEventType != CustomEventType.Meeting);

        if (searchString is not null)
        {
            predicate = predicate.And(e =>
                e.Name.ToLower().Contains(searchString) || e.Description.ToLower().Contains(searchString));
        }

        if (fromDate.HasValue)
        {
            predicate = predicate.And(e => e.StartTimeUtc >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            predicate = predicate.And(e => e.StartTimeUtc <= toDate.Value);
        }

        if (fromMinRequiredAge.HasValue)
        {
            predicate = predicate.And(e => e.MinRequiredAge >= fromMinRequiredAge);
        }

        if (toMinRequiredAge.HasValue)
        {
            predicate = predicate.And(e => e.MinRequiredAge <= toMinRequiredAge);
        }

        if (fromTicketPrice.HasValue)
        {
            predicate = predicate.And(e => e.TicketPrice >= fromTicketPrice);
        }

        if (toTicketPrice.HasValue)
        {
            predicate = predicate.And(e => e.TicketPrice <= toTicketPrice);
        }

        if (customEventTypes is not null)
        {
            predicate = predicate.And(e => customEventTypes.Contains(e.CustomEventType));
        }

        if (customEventLocationTypes is not null)
        {
            predicate = predicate.And(e => customEventLocationTypes.Contains(e.CustomEventLocationType));
        }

        if (customEventCategories is not null)
        {
            var pred = PredicateBuilder.New<CustomEvent>(false);

            foreach (var category in customEventCategories)
            {
                pred = pred.Or(c => c.CustomEventCategories.Contains(category));
            }

            predicate = predicate.And(pred);
        }

        return await _unitOfWork.CustomEvents.GetAllAsync(predicate);
    }

    public async Task<Result<AttendanceInvite>> SetInviteStatus(AttendanceInvite invite, InviteStatus newStatus)
    {
        invite.InviteStatus = newStatus;
        _unitOfWork.AttendanceInvites.Update(invite);

        await _unitOfWork.CompleteAsync();

        return Result.Success(invite);
    }

    public async Task<Result> AddAttendantToEvent(int customEventId, int guestId)
    {
        var customEvent =
            await _unitOfWork.CustomEvents.GetFirstOrDefaultAsync(e => e.Id == customEventId,
                include: i => i.Include(e => e.Attendants), disableTracking: false);

        if (customEvent is null)
        {
            return Result.Failure("Event doesnt exist");
        }

        var guest = await _unitOfWork.Guests.GetFirstOrDefaultAsync(g => g.Id == guestId, disableTracking: false);

        if (guest is null)
        {
            return Result.Failure("Guest with this id doesnt exist");
        }

        if (customEvent.TicketPrice > guest.Balance)
        {
            return Result.Failure("Not enough money on your balance to attend.");
        }

        var organizer =
            await _unitOfWork.Organizers.GetFirstOrDefaultAsync(o => o.Id == customEvent.OrganizerId,
                disableTracking: false);

        var fee = Math.Round(customEvent.TicketPrice * 0.05m, 2);

        guest.Balance -= customEvent.TicketPrice;
        organizer.Balance += customEvent.TicketPrice - fee;

        //TODO: set fee to GatherUs

        customEvent.Attendants.Add(guest);

        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<AttendanceInvite> GetInvite(int inviteId, int? organizerId = null)
    {
        if (organizerId is null)
        {
            return await _unitOfWork.AttendanceInvites.GetFirstOrDefaultAsync(i => i.Id == inviteId);
        }

        return await _unitOfWork.AttendanceInvites.GetFirstOrDefaultAsync(i =>
            i.Id == inviteId && i.CustomEvent.OrganizerId == organizerId);
    }

    public async Task DeleteInvite(int inviteId)
    {
        await _unitOfWork.AttendanceInvites.Remove(inviteId);
        await _unitOfWork.CompleteAsync();
    }
}

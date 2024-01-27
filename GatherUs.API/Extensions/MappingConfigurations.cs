using AutoMapper;
using GatherUs.API.DTO.Event;
using GatherUs.API.DTO.Guest;
using GatherUs.DAL.Models;

namespace GatherUs.API.Extensions;

public class MappingConfigurations : Profile
{
    public MappingConfigurations()
    {
        CreateMap<CustomEvent, CustomEventDto>();
        
        CreateMap<AttendanceInvite, AttendanceInviteDto>();

        CreateMap<Guest, GuestDto>();
    }
}
using GatherUs.DAL.Models;

namespace GatherUs.Core.Mailing.Dto;

public class AttendanceDto
{
    public readonly AttendanceInvite AttendanceInvite;

    public AttendanceDto(AttendanceInvite attendanceInvite)
    {
        AttendanceInvite = attendanceInvite;
    }
}
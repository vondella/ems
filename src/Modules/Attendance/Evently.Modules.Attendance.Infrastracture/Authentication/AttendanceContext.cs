using Evently.Common.Application.Exceptions;
using Evently.Common.Infrastracture.Authentication;
using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Evently.Modules.Attendance.Infrastracture.Authentication;

internal sealed class AttendanceContext(IHttpContextAccessor httpContextAccessor) : IAttendanceContext
{
    public Guid AttendeeId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new EventlyException("User identifier is unavailable");
}
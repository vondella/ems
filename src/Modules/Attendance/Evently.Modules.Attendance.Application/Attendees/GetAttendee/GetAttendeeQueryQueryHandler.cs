using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Attendee;
using System.Data.Common;

namespace Evently.Modules.Attendance.Application.Attendees.GetAttendee;


internal sealed class GetAttendeeQueryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetAttendeeQuery, AttendeeResponse>
{
    public async  Task<ResponseWrapper<AttendeeResponse>> Handle(GetAttendeeQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(AttendeeResponse.Id)},
                 email AS {nameof(AttendeeResponse.Email)},
                 first_name AS {nameof(AttendeeResponse.FirstName)},
                 last_name AS {nameof(AttendeeResponse.LastName)}
             FROM attendance.attendees
             WHERE id = @CustomerId
             """;
        AttendeeResponse? customer = await connection.QuerySingleOrDefaultAsync<AttendeeResponse>(sql, request);

        if (customer is null)
        {
            return ResponseWrapper<AttendeeResponse>.Fail(AttendeeErrors.NotFound(request.CustomerId));
        }

        return ResponseWrapper<AttendeeResponse>.Success(customer);
    }
}
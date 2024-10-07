﻿using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendee;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;

namespace Evently.Modules.Attendance.Application.Tickets.CreateTicket;

internal sealed class CreateTicketCommandHandler(
    IAttendeeRepository attendeeRepository,
    IEventRepository eventRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketCommand>
{
    public async  Task<ResponseWrapper> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        Attendee? attendee = await attendeeRepository.GetAsync(request.AttendeeId, cancellationToken);

        if (attendee is null)
        {
            return ResponseWrapper<Guid>.Fail(AttendeeErrors.NotFound(request.AttendeeId));
        }

        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return ResponseWrapper<Guid>.Fail(EventErrors.NotFound(request.EventId));
        }

        var ticket = Ticket.Create(request.TicketId, attendee, @event, request.Code);

        ticketRepository.Insert(ticket);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Guid>.Success();
    }
}
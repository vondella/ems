using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events;

public sealed class Event:Entity
{
    public Event()
    {

    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public EventStatus Status { get; private set; }

    public static ResponseWrapper<Event> Create(string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
        {
           return ResponseWrapper<Event>.Fail("Invalid date range (ending date must be greater than starting date)");
        }
        var @event = new Event
        {
            Id = Guid.NewGuid(),
            Title=title,
            Description=description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc=endsAtUtc,
            Status = EventStatus.Draft
        };
        @event.Raise(new EventCreatedDomainEvent(@event.Id));
        return  ResponseWrapper<Event>.Success(@event,"event created successfully");
    }

    public ResponseWrapper Publish()
    {
        if (Status != EventStatus.Draft)
        {
            return ResponseWrapper<Event>.Fail("Not Drafted");
        }

        Status = EventStatus.Published;
        Raise(new EventPublishedDomainEvent(Id));
        return ResponseWrapper<Event>.Success(this,"Event published successfully");
    }

    public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        if (StartsAtUtc == startsAtUtc && EndsAtUtc == endsAtUtc)
        {
            return;
        }

        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        Raise(new EventRescheduledDomainEvent(Id,startsAtUtc,endsAtUtc));
    }

    public ResponseWrapper Cancel(DateTime utcNow)
    {
        if (Status == EventStatus.Canceled)
        {
            return ResponseWrapper<Event>.Fail(" Event Already cancelled");
        }

        if (StartsAtUtc < utcNow)
        {
            return ResponseWrapper<Event>.Fail("Event Already started");
        }

        Status = EventStatus.Canceled;
        Raise(new EventCanceledDomainEvent(Id));
        return ResponseWrapper<Event>.Success(this);
    }

}
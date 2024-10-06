
using Evently.Common.Domain;
using Evently.Modules.Events.Domain.Categories;

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

    public static ResponseWrapper<Event> Create(
        Category category,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
        {
           return ResponseWrapper<Event>.Fail(EventErrors.EndDatePrecedesStartDate);
        }
        var @event = new Event
        {
            Id = Guid.NewGuid(),
            Title=title,
            CategoryId = category.Id,
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
            return ResponseWrapper<Event>.Fail(EventErrors.NotDraft);
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
            return ResponseWrapper<Event>.Fail(EventErrors.AlreadyCanceled);
        }

        if (StartsAtUtc < utcNow)
        {
            return ResponseWrapper<Event>.Fail(EventErrors.AlreadyStarted);
        }

        Status = EventStatus.Canceled;
        Raise(new EventCanceledDomainEvent(Id));
        return ResponseWrapper<Event>.Success(this);
    }

}
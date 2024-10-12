using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.test.Abstraction;
using FluentAssertions;

namespace Evently.Modules.Events.test.Events;

public class EventTest:BaseTest
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenEndDatesPrecedeStartDate()
    {
        // arrange
        Category category=Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;
        DateTime endsDateUtc = startAtUtc.AddMinutes(-1);

        // act
       var responseWrapper= Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, endsDateUtc);

        // assert
        responseWrapper.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        // arrange
        Category category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;

        // act
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, null);

        Event @event = responseWrapper.ResponseData;
        // assert
        EventCreatedDomainEvent domainEvent=AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);

        domainEvent.Id.Should().Be(@event.Id);
    }

    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventNotDrafted()
    {
        // arrange
        Category category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;

        // act
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, null);

        Event @event = responseWrapper.ResponseData;
        @event.Publish();
        // assert
        var publishResult = @event.Publish();
        publishResult.Error.Should().Be(EventErrors.NotDraft);
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        // arrange
        Category category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;

        // act
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, null);

        Event @event = responseWrapper.ResponseData;
        @event.Reschedule(startAtUtc.AddDays(1),startAtUtc.AddDays(2));

        // assert
        EventRescheduledDomainEvent eventRescheduledDomainEvent =
            AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);

        eventRescheduledDomainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        // arrange
        Category category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;

        // act
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, null);

        Event @event = responseWrapper.ResponseData;
        @event.Cancel(startAtUtc.AddDays(-1));

        // assert
        EventCanceledDomainEvent eventCanceledDomainEvent =
            AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event);

        eventCanceledDomainEvent.EventId.Should().Be(@event.Id);
    }
    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        // arrange
        Category category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;

        // act
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, null);

        Event @event = responseWrapper.ResponseData;
        var result=@event.Cancel(startAtUtc.AddMinutes(1));

        // assert
        result.Error.Should().Be(EventErrors.AlreadyStarted);
    }
}
namespace Evently.Modules.Users.IntegrationEvent;

public sealed class UserProfileUpdatedIntegrationEvent : Common.Application.EventBus.IntegrationEvent
{
    public UserProfileUpdatedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        string firstName,
        string lastName)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }
}
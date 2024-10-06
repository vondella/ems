using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain;

public sealed class UserRegisteredDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
}
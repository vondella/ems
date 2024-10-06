namespace Evently.Common.Infrastracture.Outbox;

public sealed class OutboxMessageConsumer(Guid outboxMessageId, string name)
{
    public Guid OutboxMessageId { get; init; } = outboxMessageId;

    public string Name { get; init; } = name;
}
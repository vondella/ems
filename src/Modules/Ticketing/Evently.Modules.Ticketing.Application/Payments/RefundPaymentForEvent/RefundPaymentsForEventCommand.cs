using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentForEvent;

public sealed record RefundPaymentsForEventCommand(Guid EventId) : ICommand;
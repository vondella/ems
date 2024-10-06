using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentForEvent;

internal sealed class RefundPaymentsForEventCommandValidator : AbstractValidator<RefundPaymentsForEventCommand>
{
    public RefundPaymentsForEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}
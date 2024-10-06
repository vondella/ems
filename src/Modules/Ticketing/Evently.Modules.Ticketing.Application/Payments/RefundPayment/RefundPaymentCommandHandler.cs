using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Payments;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPayment;

internal sealed class RefundPaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RefundPaymentCommand>
{
    public async  Task<ResponseWrapper> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        Payment? payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            return ResponseWrapper<Guid>.Fail(PaymentErrors.NotFound(request.PaymentId));
        }

        var result = payment.Refund(request.Amount);

        if (!result.IsSuccessful)
        {
            return ResponseWrapper<Guid>.Fail(PaymentErrors.NotEnoughFunds);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Guid>.Success();
    }
}
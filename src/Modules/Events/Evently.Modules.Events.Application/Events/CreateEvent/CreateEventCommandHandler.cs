using Evently.Common.Application.Clock;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using MediatR;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(IDateTimeProvider dateTimeProvider,ICategoryRepository categoryRepository,
    IEventRepository _eventRepository, IUnitOfWork unitOfWork) :ICommandHandler<CreateEventCommand,Guid>
{
    public async Task<ResponseWrapper<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return ResponseWrapper<Guid>.Fail(EventErrors.StartDateInPast);
        }

        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return ResponseWrapper<Guid>.Fail(CategoryErrors.NotFound(request.CategoryId));
        }
        var @event = Event.Create(category, request.Title, request.Description, request.Location, request.StartsAtUtc, request.EndsAtUtc);

        if (!@event.IsSuccessful)
        {
            return ResponseWrapper<Guid>.Fail(@event.Error);
        }
        _eventRepository.Insert(@event.ResponseData);
        await unitOfWork.SaveChangesAsync();
        return ResponseWrapper<Guid>.Success(@event.ResponseData.Id);
    }


}

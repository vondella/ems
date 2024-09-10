using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.ArchiveCategory;

internal sealed class ArchiveCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveCategoryCommand>
{
    public async Task<ResponseWrapper> Handle(ArchiveCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return ResponseWrapper<Category>.Fail($"Category with id {request.CategoryId} is not found");
        }
        if (category.IsArchived)
        {
            return ResponseWrapper<Category>.Fail("Already archived");
        }

        category.Archive();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ResponseWrapper<Category>.Success();
    }
}
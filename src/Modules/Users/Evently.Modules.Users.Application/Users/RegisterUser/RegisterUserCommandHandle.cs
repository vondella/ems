using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.PublicApi;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(IIdentityProviderService identityProviderService,
    IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<ResponseWrapper<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await identityProviderService.RegisterUserAsync(
            new UserModel(request.Email, request.Password, request.FirstName, request.LastName),
            cancellationToken);

        if (!result.IsSuccessful)
        {
            return ResponseWrapper<Guid>.Fail(result.Error);
        }
        var user = User.Create(request.Email, request.FirstName, request.LastName,result.ResponseData);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        //await ticketingApi.CreateCustomerAsync(user.Id, user.Email, user.FirstName, user.LastName, cancellationToken);
        return  ResponseWrapper<Guid>.Success(user.Id);
    }
}
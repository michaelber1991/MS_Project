using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(int Id, string Name, string Email, string Username, string GlobalName)
    : IRequest<Result<User>>;

public class UpdateUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.Id);
        if (user is null)
            return Result<User>.Fail(["User not found"]);

        user.Name = request.Name;

        await unitOfWork.Users.UpdateAsync(user);
        await unitOfWork.CommitAsync();

        return Result<User>.Ok(user, "User updated");
    }
}
using Clean.Application.Interfaces;
using Clean.Application.Models;
using MediatR;

namespace Clean.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(int UserId) : IRequest<Result<string>>;

public class DeleteUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var deletedId = await unitOfWork.Users.DeleteAsync(request.UserId);

        if (deletedId == null)
        {
            var errors = new List<string> { "User not found or could not be deleted" };
            return Result<string>.Fail(errors);
        }

        await unitOfWork.CommitAsync();

        return Result<string>.Ok($"User with ID {deletedId} deleted");
    }
}
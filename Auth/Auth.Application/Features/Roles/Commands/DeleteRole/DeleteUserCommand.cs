using Auth.Application.Interfaces;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(int RoleId) : IRequest<Result<string>>;

public class DeleteRoleHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var deletedId = await unitOfWork.Roles.DeleteAsync(request.RoleId);

        if (deletedId == null)
        {
            var errors = new List<string> { "Application not found or could not be deleted" };
            return Result<string>.Fail(errors);
        }

        await unitOfWork.CommitAsync();

        return Result<string>.Ok($"Application with ID {deletedId} deleted");
    }
}
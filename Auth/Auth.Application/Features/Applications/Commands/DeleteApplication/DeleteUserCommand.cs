using Auth.Application.Interfaces;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Applications.Commands.DeleteApplication;

public record DeleteApplicationCommand(int UserId) : IRequest<Result<string>>;

public class DeleteApplicationHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteApplicationCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var deletedId = await unitOfWork.Applications.DeleteAsync(request.UserId);

        if (deletedId == null)
        {
            var errors = new List<string> { "Application not found or could not be deleted" };
            return Result<string>.Fail(errors);
        }

        await unitOfWork.CommitAsync();

        return Result<string>.Ok($"Application with ID {deletedId} deleted");
    }
}
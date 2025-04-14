using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Users.Commands.AssignUserToApplication;

public record AssignUserToApplicationCommand(int UserId, int ApplicationId) : IRequest<Result<string>>;

public class AssignUserToApplicationHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AssignUserToApplicationCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AssignUserToApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null) return Result<string>.Fail(["User not found"]);

        var application = await unitOfWork.Applications.GetByIdAsync(request.ApplicationId);
        if (application == null) return Result<string>.Fail(["Application not found"]);

        var assignment = new UserApplication
        {
            UserId = request.UserId,
            ApplicationId = request.ApplicationId,
            CreatedAt = DateTime.UtcNow
        };

        user.UserApplications.Add(assignment);
        await unitOfWork.CommitAsync();

        return Result<string>.Ok("User assigned to application");
    }
}
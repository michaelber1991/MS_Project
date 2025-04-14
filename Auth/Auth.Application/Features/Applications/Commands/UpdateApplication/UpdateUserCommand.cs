using Auth.Application.Interfaces;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Applications.Commands.UpdateApplication;

public record UpdateApplicationCommand(int Id, string Name, string Description)
    : IRequest<Result<Domain.Entities.Application>>;

public class UpdateApplicationHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateApplicationCommand, Result<Domain.Entities.Application>>
{
    public async Task<Result<Domain.Entities.Application>> Handle(UpdateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var application = await unitOfWork.Applications.GetByIdAsync(request.Id);
        if (application is null)
            return Result<Domain.Entities.Application>.Fail(["Application not found"]);

        application.Name = request.Name;
        application.Description = request.Description;

        await unitOfWork.Applications.UpdateAsync(application);
        await unitOfWork.CommitAsync();

        return Result<Domain.Entities.Application>.Ok(application, "Application updated");
    }
}
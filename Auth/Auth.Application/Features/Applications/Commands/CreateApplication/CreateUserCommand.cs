using Auth.Application.Interfaces;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Applications.Commands.CreateApplication;

public record CreateApplicationCommand(string Name, string Description)
    : IRequest<Result<Domain.Entities.Application>>;

public class CreateApplicationHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateApplicationCommand, Result<Domain.Entities.Application>>
{
    public async Task<Result<Domain.Entities.Application>> Handle(CreateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var application = new Domain.Entities.Application
        {
            Name = request.Name,
            Description = request.Description
        };

        await unitOfWork.Applications.AddAsync(application);
        await unitOfWork.CommitAsync();

        return Result<Domain.Entities.Application>.Ok(application, "User created");
    }
}
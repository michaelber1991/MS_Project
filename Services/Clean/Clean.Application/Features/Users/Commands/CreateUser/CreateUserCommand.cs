using Clean.Application.Interfaces;
using Clean.Application.Models;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string Name, string Email) : IRequest<Result<User>>;

public class CreateUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.CommitAsync();

        return Result<User>.Ok(user, "User created");
    }
}
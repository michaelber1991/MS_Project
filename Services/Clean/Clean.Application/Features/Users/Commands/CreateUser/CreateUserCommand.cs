using Clean.Application.Interfaces;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string Name, string Email) : IRequest<int>;

public class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        await _userRepository.AddAsync(user);
        return user.Id;
    }
}
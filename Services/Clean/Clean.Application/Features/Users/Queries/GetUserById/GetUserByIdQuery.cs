using Clean.Application.Interfaces;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Queries.GetUserById;
public record GetUserByIdQuery(int Id) : IRequest<User?>;

public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(request.Id);
    }
}
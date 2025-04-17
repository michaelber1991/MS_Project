using Clean.Application.Interfaces.Repositories;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<User?>;

public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync(request.Id);
    }
}


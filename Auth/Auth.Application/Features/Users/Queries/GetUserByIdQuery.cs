using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Features.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<User?>;

public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync(request.Id);
    }
}
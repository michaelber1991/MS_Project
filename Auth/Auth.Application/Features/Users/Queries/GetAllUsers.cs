using Auth.Application.Common;
using Auth.Application.Features.Common.Queries;
using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllQuery<User>, PagedResult<User>>
{
    public Task<PagedResult<User>> Handle(GetAllQuery<User> request, CancellationToken cancellationToken)
    {
        var result = userRepository.GetAllPaginatedFiltered(request.QueryParams);
        return Task.FromResult(new PagedResult<User>(result.Data, result.TotalCount));
    }
}
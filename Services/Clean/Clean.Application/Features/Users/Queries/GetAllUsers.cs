using Clean.Application.Common;
using Clean.Application.Features.Common.Queries;
using Clean.Application.Interfaces.Repositories;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllQuery<User>, PagedResult<User>>
{
    public async Task<PagedResult<User>> Handle(GetAllQuery<User> request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetAllPaginatedFiltered(request.QueryParams);
        return new PagedResult<User>(result.Data, result.TotalCount);
    }
}


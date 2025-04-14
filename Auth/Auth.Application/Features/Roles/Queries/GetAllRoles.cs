using Auth.Application.Common;
using Auth.Application.Features.Common.Queries;
using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Features.Roles.Queries;

public class GetAllRolessQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetAllQuery<Role>, PagedResult<Role>>
{
    public Task<PagedResult<Role>> Handle(GetAllQuery<Role> request, CancellationToken cancellationToken)
    {
        var result = roleRepository.GetAllPaginatedFiltered(request.QueryParams);
        return Task.FromResult(new PagedResult<Role>(result.Data, result.TotalCount));
    }
}
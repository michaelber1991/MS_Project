using Auth.Application.Common;
using Auth.Application.Features.Common.Queries;
using Auth.Application.Interfaces.Repositories;
using MediatR;

namespace Auth.Application.Features.Applications.Queries;

public class GetAllApplicationsQueryHandler(IApplicationRepository applicationRepository)
    : IRequestHandler<GetAllQuery<Domain.Entities.Application>, PagedResult<Domain.Entities.Application>>
{
    public Task<PagedResult<Domain.Entities.Application>> Handle(GetAllQuery<Domain.Entities.Application> request,
        CancellationToken cancellationToken)
    {
        var result = applicationRepository.GetAllPaginatedFiltered(request.QueryParams);
        return Task.FromResult(new PagedResult<Domain.Entities.Application>(result.Data, result.TotalCount));
    }
}
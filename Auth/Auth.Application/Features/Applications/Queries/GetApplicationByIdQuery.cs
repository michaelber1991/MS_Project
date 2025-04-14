using Auth.Application.Interfaces.Repositories;
using MediatR;

namespace Auth.Application.Features.Applications.Queries;

public record GetApplicationByIdQuery(int Id) : IRequest<Domain.Entities.Application?>;

public class GetApplicationByIdHandler(IApplicationRepository applicationRepository)
    : IRequestHandler<GetApplicationByIdQuery, Domain.Entities.Application?>
{
    public async Task<Domain.Entities.Application?> Handle(GetApplicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await applicationRepository.GetByIdAsync(request.Id);
    }
}
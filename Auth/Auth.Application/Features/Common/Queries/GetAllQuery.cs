using Auth.Application.Common;
using MediatR;

namespace Auth.Application.Features.Common.Queries;

public class GetAllQuery<T>(QueryParams queryParams) : IRequest<PagedResult<T>>
{
    public QueryParams QueryParams { get; set; } = queryParams;
}
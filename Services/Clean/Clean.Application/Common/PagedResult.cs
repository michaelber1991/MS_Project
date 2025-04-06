namespace Clean.Application.Common;

public class PagedResult<T>(IEnumerable<T> data, int totalCount)
{
    public IEnumerable<T> Data { get; set; } = data;
    public int TotalCount { get; set; } = totalCount;
}
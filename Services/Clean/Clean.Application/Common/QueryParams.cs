namespace Clean.Application.Common;

public class Filter
{
    public string Property { get; set; }
    public object Value { get; set; }
    public string Type { get; set; }
}

public class Order
{
    public required string Property { get; set; }
    public bool Ascending { get; set; }
}

public class QueryParams
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? Filters { get; set; }
    public string? Orders { get; set; }
}
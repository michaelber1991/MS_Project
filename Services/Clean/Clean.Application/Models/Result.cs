namespace Clean.Application.Models;

public class Result<T>
{
    public bool Success { get; private init; }
    public string? Message { get; private init; }
    public T? Data { get; private init; }
    public List<string> Errors { get; private init; } = new();

    public static Result<T> Ok(T data, string? message = null)
    {
        return new Result<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "Operation succeeded",
            Errors = new List<string>()
        };
    }

    public static Result<T> Fail(List<string> errors, string? message = null)
    {
        return new Result<T>
        {
            Success = false,
            Errors = errors,
            Message = message ?? "Operation failed"
        };
    }
}
namespace Clean.Application.Models;

public class Result<T>
{
    public bool Succeeded { get; private init; }
    public string? Message { get; private init; }
    public T? Data { get; private init; }
    public List<string> Errors { get; private init; } = new();

    public static Result<T> Ok(T data, string? message = null)
    {
        return new Result<T>
        {
            Succeeded = true,
            Data = data,
            Message = message ?? "Operation succeeded",
            Errors = new List<string>()
        };
    }

    public static Result<T> Fail(List<string> errors, string? message = null)
    {
        return new Result<T>
        {
            Succeeded = false,
            Errors = errors,
            Message = message ?? "Operation failed"
        };
    }
}
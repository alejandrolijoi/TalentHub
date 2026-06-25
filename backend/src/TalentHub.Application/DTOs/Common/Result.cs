namespace TalentHub.Application.DTOs.Common;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public string? Error { get; protected set; }

    protected Result() { }

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string error) => new() { IsSuccess = false, Error = error };
}

public class Result<T> : Result
{
    public T? Value { get; protected set; }

    protected Result() { }

    public new static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public new static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
}

public class PaginatedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

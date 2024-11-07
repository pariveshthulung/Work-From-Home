using System;

namespace Clean.Application.Wrappers;

public class BaseResult
{
    public bool Success { get; set; }
    public List<Error> Errors { get; set; }

    public static BaseResult Ok() => new() { Success = true };

    public static BaseResult Failure() => new() { Success = false };

    public static BaseResult Failure(Error error) => new() { Success = false, Errors = [error] };

    public static BaseResult Failure(IEnumerable<Error> errors) =>
        new() { Success = false, Errors = [.. errors] };

    public static implicit operator BaseResult(Error error) =>
        new() { Success = true, Errors = [error] };

    public static implicit operator BaseResult(List<Error> errors) =>
        new() { Success = false, Errors = errors };

    public BaseResult AddError(Error error)
    {
        Errors ??= [];
        Errors.Add(error);
        Success = false;
        return this;
    }
}

public class BaseResult<TData> : BaseResult
{
    public TData Data { get; set; }

    public static BaseResult<TData> Ok(TData data) => new() { Success = true, Data = data };

    public static new BaseResult<TData> Failure() => new() { Success = false };

    public static new BaseResult<TData> Failure(Error error) =>
        new() { Success = false, Errors = [error] };

    public static new BaseResult<TData> Failure(IEnumerable<Error> error) =>
        new() { Success = false, Errors = [.. error] };

    public static implicit operator BaseResult<TData>(Error error) =>
        new() { Success = false, Errors = [error] };

    public static implicit operator BaseResult<TData>(List<Error> errors) =>
        new() { Success = false, Errors = [.. errors] };
}

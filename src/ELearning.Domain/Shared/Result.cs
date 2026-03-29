using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared;
public class Result
{
    protected Result(bool isSuccuss, Error error)
    {
        if (isSuccuss && error != null ||
            !isSuccuss && error == Error.None)
        {
            throw new ArgumentException("Invalid arguments");
        }

        IsSuccuss = isSuccuss;
        Error = error;
    }

    public bool IsSuccuss { get; }
    public bool IsFailure => !IsSuccuss;
    public Error? Error { get; }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    public static implicit operator Result(Error error) => Failure(error); 
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    public TValue Value =>
        IsSuccuss ? _value : throw new InvalidOperationException("Cannot access value of failed result");

    private Result(TValue value) : base(true, Error.None)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
        _value = default;
    }

    public static Result<TValue> Succuss(TValue value) => new(value);
    public new static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Succuss(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
}

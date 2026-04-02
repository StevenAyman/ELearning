using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared;
public sealed record Money
{
    public static Money Zero() => new Money(0);
    public decimal Amount { get; init; }
    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("The money amount should be positive number at least zero");
        }
        Amount = amount;
    }

    public static Money operator +(Money left, Money right) => new Money(left.Amount + right.Amount);
    public static Money operator -(Money left, Money right) => new Money(left.Amount - right.Amount);
    public static Money operator *(Money left, Money right) => new Money(right.Amount * left.Amount);
    public static bool operator <(Money left, Money right) => left?.Amount < right?.Amount;
    public static bool operator >(Money left, Money right) => left?.Amount > right?.Amount;
    public static bool operator <=(Money left, Money right) => left?.Amount <= right?.Amount;
    public static bool operator >=(Money left, Money right) => left?.Amount >= right?.Amount;

}

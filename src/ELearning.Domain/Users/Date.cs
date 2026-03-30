using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Users;
public sealed record Date
{
    public DateOnly Value { get; private set; }

    private Date(DateOnly value)
    {
        Value = value;
    }

    public static Date Create(DateOnly date)
    {
        if (date > DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            throw new ArgumentException("Date shouldn't be in the future");
        }
        return new Date(date);
    }
}

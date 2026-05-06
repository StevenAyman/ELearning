using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared;
public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

public static class Errors
{
    public static readonly Error DatabaseError = new Error(
                "Error",
                "An Error has been occurred while trying to save at database");
}

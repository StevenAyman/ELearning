using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Purchases;
public static class PaidCodeErrors
{
    public static readonly Error NotFound = new(
        "PaidCode.NotFound",
        "Sorry, This code is not found");

    public static readonly Error Expired = new(
        "PaidCode.Expired",
        "Sorry, This code is expired");

    public static readonly Error Used = new(
        "PaidCode.Used",
        "Error. This code is used before");
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Purchases;
public static class PurchaseErrors
{
    public static readonly Error PurchaseNotFound = new(
        "Purchase.NotFound",
        "Error. You didn't buy this session");
}

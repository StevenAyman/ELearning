using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Purchases;
public sealed class PurchaseMethod : BaseEntity
{
    private PurchaseMethod() { }

    public PurchaseMethod(
        string id,
        PaymentType type) : base(id)
    {
        Type = type;
    }

    public PaymentType Type { get; private set; } 
}

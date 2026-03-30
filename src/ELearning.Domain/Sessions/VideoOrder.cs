using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Sessions;
public sealed record VideoOrder
{
    public int Value { get; private set; }

    private VideoOrder(int order)
    {
        Value = order;
    }

    public static VideoOrder Create(int order)
    {
        if (order <= 0)
        {
            throw new ApplicationException("Invalid value, order should be 1 and above");
        }

        return new VideoOrder(order);
    }
}

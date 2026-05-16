using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Discounts.DTOs;
public sealed record DiscountAreaResponse
{
    public int Id { get; init; }
    public string Area { get; init; }
}

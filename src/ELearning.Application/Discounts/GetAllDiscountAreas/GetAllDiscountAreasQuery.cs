using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;

namespace ELearning.Application.Discounts.GetAllDiscountAreas;
public sealed record GetAllDiscountAreasQuery() : IQuery<IEnumerable<DiscountAreaResponse>>;

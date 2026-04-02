using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Discounts.Events;
public sealed record DiscountCodeUsedDomainEvent(string DiscountCodeId) : IDomainEvent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Purchases.Events;
public sealed record PurchaseFailedDomainEvent(string PurchaseId) : IDomainEvent;

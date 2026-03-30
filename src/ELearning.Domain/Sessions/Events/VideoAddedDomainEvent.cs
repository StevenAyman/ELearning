using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Sessions.Events;
public sealed record VideoAddedDomainEvent(string VideoId) : IDomainEvent;

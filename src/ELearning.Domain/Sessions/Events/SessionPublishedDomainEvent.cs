using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using MediatR;

namespace ELearning.Domain.Sessions.Events;
public sealed record SessionPublishedDomainEvent(string SessionId) : IDomainEvent;

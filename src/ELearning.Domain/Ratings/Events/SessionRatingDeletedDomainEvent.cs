using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Ratings.Events;
public sealed record SessionRatingDeletedDomainEvent(string SessionId, Rating StudentRating) : IDomainEvent;

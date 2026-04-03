using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Ratings.Events;
public sealed record InstructorRatedDomainEvent(string InstructorId, Rating Rating) : IDomainEvent;

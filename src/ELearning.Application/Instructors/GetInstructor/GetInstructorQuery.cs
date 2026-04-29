using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Instructors.DTOs;

namespace ELearning.Application.Instructors.GetInstructor;
public sealed record GetInstructorQuery(string Id) : IQuery<InstructorWithSessionsDto>;

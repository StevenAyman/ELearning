using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Instructors.DTOs;

namespace ELearning.Application.Instructors.GetAllInstructors;
public sealed record GetAllInstructorsQuery(string? ClassId, string? SubjectId) : IQuery<IEnumerable<InstructorDto>>;

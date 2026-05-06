using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Instructors.RemoveAssignedInstructor;
public sealed record RemoveAssignedInstructorCommand(string InstructorId, string ClassId, string SubjectId) : ICommand;

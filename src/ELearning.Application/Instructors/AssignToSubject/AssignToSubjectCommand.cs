using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Instructors.AssignToSubject;
public sealed record AssignToSubjectCommand(string InstructorId, string ClassId, string SubjectId) : ICommand;

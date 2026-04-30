using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Assistants.AssignNewInstructor;
public sealed record AssignNewInstructorCommand(string Id, string InstructorId): ICommand;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Instructors.UpdateInstructor;
public sealed record UpdateInstructorCommand(string Id , string Bio) : ICommand;

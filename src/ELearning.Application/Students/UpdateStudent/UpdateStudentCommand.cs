using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Students.UpdateStudent;
public sealed record UpdateStudentCommand(string Id, string ClassId) : ICommand;

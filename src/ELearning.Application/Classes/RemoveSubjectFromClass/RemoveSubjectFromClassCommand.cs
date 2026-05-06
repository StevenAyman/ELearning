using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Classes.RemoveSubjectFromClass;
public sealed record RemoveSubjectFromClassCommand(string ClassId, string SubjectId) : ICommand;

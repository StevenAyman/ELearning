using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Classes.AddSubjectToClass;
public sealed record AddSubjectToClassCommand(string ClassId, string SubjectId) : ICommand;

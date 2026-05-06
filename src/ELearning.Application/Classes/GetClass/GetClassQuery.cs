using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Classes.DTOs;

namespace ELearning.Application.Classes.GetClass;
public sealed record GetClassQuery(string Id) : IQuery<ClassWithSubjectsDto>;

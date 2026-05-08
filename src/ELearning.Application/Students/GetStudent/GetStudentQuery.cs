using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Students.DTOs;

namespace ELearning.Application.Students.GetStudent;
public sealed record GetStudentQuery(string Id) : IQuery<StudentDto>;

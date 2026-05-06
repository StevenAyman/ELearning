using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Users;

namespace ELearning.Application.Students.RegisterStudent;
public sealed record RegisterStudentCommand(
    FirstName FirstName,
    LastName LastName,
    Email Email,
    string DateOfBirth,
    string City,
    string IdentityId,
    string Class) : ICommand;

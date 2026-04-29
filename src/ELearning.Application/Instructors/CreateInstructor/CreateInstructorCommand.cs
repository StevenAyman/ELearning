using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Users;

namespace ELearning.Application.Instructors.CreateInstructor;
public sealed record CreateInstructorCommand(
    FirstName FirstName,
    LastName LastName,
    Email Email,
    string DateOfBirth,
    string City,
    string IdentityId,
    string SubjectId) : ICommand;

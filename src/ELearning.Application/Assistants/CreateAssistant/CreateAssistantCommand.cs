using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Users;

namespace ELearning.Application.Assistants.CreateAssistant;
public sealed record CreateAssistantCommand(
    FirstName FirstName,
    LastName LastName,
    Email Email,
    string DateOfBirth,
    string City,
    string IdentityId) : ICommand;




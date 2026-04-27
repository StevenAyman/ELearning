using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Users.UpdateUserEmail;
public sealed record UpdateUserEmailCommand(string OldEmail, string NewEmail) : ICommand;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Users;

namespace ELearning.Application.Users.UpdateUserProfile;
public sealed record UpdateUserProfileCommand(FirstName FirstName, LastName LastName, string City, string BirthDate, string IdentityId): ICommand;

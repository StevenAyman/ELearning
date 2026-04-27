using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Users.GetUserWithIdentity;
public sealed record GetUserWithIdentityQuery(string IdentityId) : IQuery<UserDto>;

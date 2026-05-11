using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.PaidCodes.RedeemPaidCode;
public sealed record RedeemPaidCodeCommand(string Code, string StudentId) : ICommand;

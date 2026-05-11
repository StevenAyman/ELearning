using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.PaidCodes.DTOs;
using ELearning.Domain.Purchases;

namespace ELearning.Application.PaidCodes.GeneratePaidCodes;
public sealed record GeneratePaidCodesCommand(int Count, decimal Balance) : ICommand<IEnumerable<PaidCodeResponse>>;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Discounts.UpdateDiscountArea;
public sealed record UpdateDiscountAreaCommand(int Id, string NewAreaName) : ICommand;

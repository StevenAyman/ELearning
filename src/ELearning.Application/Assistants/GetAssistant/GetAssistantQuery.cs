using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Assistants.DTOs;

namespace ELearning.Application.Assistants.GetAssistant;
public sealed record GetAssistantQuery(string Id) : IQuery<AssistantResponse>;

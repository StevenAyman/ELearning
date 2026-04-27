using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using MediatR;

namespace ELearning.Application.Abstractions.Messaging;
public interface ICommand : IRequest<Result>, IBaseRequest
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>> , IBaseRequest
{
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using MediatR;

namespace ELearning.Application.Abstractions.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseRequest
{
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Classes.DTOs;
using ELearning.Domain.Shared;

namespace ELearning.Application.Classes.GetAllClasses;
internal sealed class GetAllClassesQueryHandler(
    IClassReadService classReadService) : IQueryHandler<GetAllClassesQuery, IEnumerable<ClassDto>>
{
    private readonly IClassReadService _classReadService = classReadService;

    public async Task<Result<IEnumerable<ClassDto>>> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
    {
        var result = await _classReadService.GetAllAsync(cancellationToken);

        return Result<IEnumerable<ClassDto>>.Succuss(result);
    }



}

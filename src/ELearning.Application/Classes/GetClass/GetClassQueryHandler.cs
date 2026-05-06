using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Classes.DTOs;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;

namespace ELearning.Application.Classes.GetClass;
internal sealed class GetClassQueryHandler(
    IClassReadService classReadService) : IQueryHandler<GetClassQuery, ClassWithSubjectsDto>
{
    private readonly IClassReadService _classReadService = classReadService;

    public async Task<Result<ClassWithSubjectsDto>> Handle(GetClassQuery request, CancellationToken cancellationToken)
    {
        var classWithSubjects = await _classReadService.GetByIdAsync(request.Id, cancellationToken);

        if (classWithSubjects is null)
        {
            return Result<ClassWithSubjectsDto>.Failure(ClassErrors.NotFound);
        }

        return classWithSubjects;
    }
}

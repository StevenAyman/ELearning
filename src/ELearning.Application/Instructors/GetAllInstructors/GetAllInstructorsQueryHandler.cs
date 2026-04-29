using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Instructors.DTOs;
using ELearning.Domain.Shared;

namespace ELearning.Application.Instructors.GetAllInstructors;
internal sealed class GetAllInstructorsQueryHandler(
    IInstructorReadService instructorReadService) : IQueryHandler<GetAllInstructorsQuery, IEnumerable<InstructorDto>>
{
    private readonly IInstructorReadService _instructorReadService = instructorReadService;

    public async Task<Result<IEnumerable<InstructorDto>>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
    {
        var instructors = await _instructorReadService.GetAllAsync(cancellationToken);

        if (instructors is null)
        {
            return Result<IEnumerable<InstructorDto>>.Failure(
                new Error("Error", "Something went wrong while trying to fetch instructors"));
        }

        return Result<IEnumerable<InstructorDto>>.Succuss(instructors);
    }
}

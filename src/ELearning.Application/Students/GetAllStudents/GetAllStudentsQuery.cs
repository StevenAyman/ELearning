using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Common;
using ELearning.Application.Students.DTOs;

namespace ELearning.Application.Students.GetAllStudents;
public sealed record GetAllStudentsQuery(
    string? ClassId, 
    string? Search,
    string? SubjectId, 
    string? InstructorId, 
    int PageIndex,
    int PageSize,
    string Sort) : IQuery<PaginationDto<StudentDto>>;

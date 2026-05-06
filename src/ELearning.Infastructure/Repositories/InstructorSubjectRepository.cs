using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Classes;
using ELearning.Domain.Instructors;

namespace ELearning.Infastructure.Repositories;
public sealed class InstructorSubjectRepository(AppDbContext dbContext) : IInstructorSubjectRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public void Add(string instructorId, string classId, string subjectId)
    {
        var instructorSubject = new ClassesSubjectsInstructor(classId, subjectId, instructorId);
        _dbContext.Set<ClassesSubjectsInstructor>().Add(instructorSubject);
    }

    public void Delete(string instructorId, string classId, string subjectId)
    {
        var instructorSubject = new ClassesSubjectsInstructor(classId, subjectId, instructorId);
        _dbContext.Set<ClassesSubjectsInstructor>().Remove(instructorSubject);
    }
}

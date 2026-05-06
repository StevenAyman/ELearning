using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Classes;

namespace ELearning.Infastructure.Repositories;
public sealed class LearningClassRepository : Repository<LearningClass>, ILearningClassRepository
{
    public LearningClassRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public void AddSubjectToClass(string classId, string subjectId)
    {
        var classSubject = new ClassesSubject(classId, subjectId);
        _dbContext.Set<ClassesSubject>().Add(classSubject);
    }

    public void RemoveSubjectFromClass(string classId, string subjectId)
    {
        var classSubject = new ClassesSubject(classId, subjectId);
        _dbContext.Set<ClassesSubject>().Remove(classSubject);
    }
}

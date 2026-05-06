using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Classes;
public interface ILearningClassRepository
{
    Task<LearningClass?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LearningClass>> GetAllAsync(CancellationToken cancellationToken = default);
    void Delete(LearningClass entity);
    void Update(LearningClass entity);
    void Add(LearningClass entity);
    void AddSubjectToClass(string classId, string subjectId);
    void RemoveSubjectFromClass(string classId, string subjectId);
}

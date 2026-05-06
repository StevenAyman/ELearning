using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Instructors;
public interface IInstructorSubjectRepository
{
    void Add(string instructorId, string classId, string subjectId);
    void Delete(string instructorId, string classId, string subjectId);
}

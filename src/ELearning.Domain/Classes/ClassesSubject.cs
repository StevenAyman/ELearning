using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Classes;
public sealed class ClassesSubject
{
    private ClassesSubject() { }
    public ClassesSubject(string classId, string subjectId)
    {
        ClassId = classId;
        SubjectId = subjectId;
    }

    public string ClassId { get; private set; }
    public string SubjectId { get; private set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Classes;
public sealed class ClassesSubjectsInstructor
{
    private ClassesSubjectsInstructor() { }
    public ClassesSubjectsInstructor(string classId, string subjectId, string instructorId)
    {
        ClassId = classId;
        SubjectId = subjectId;
        InstructorId = instructorId;
    }
    public string ClassId { get; private set; }
    public string SubjectId { get; private set; }
    public string InstructorId { get; private set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Subjects;
public static class SubjectErrors
{
    public static readonly Error NotFound = new(
        "Subject.NotFound",
        "Subject is not found with that parameter");

    public static readonly Error Duplicate = new(
        "Subject.Duplicate",
        "Subject with this name is already exist");

}

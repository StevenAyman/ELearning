using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Students;
public static class StudentErrors
{
    public static readonly Error RegisterError = new(
        "Student.Register",
        "Something went wrong while trying to register student.");
}

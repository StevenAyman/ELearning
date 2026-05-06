using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Classes;
public static class ClassErrors
{
    public static readonly Error NotFound = new(
        "Class.NotFound",
        "This class is not found. Sorry");

    public static readonly Error IsExist = new(
        "Class.IsExist",
        "Sorry but a class with that name is already exist");

    public static readonly Error SubjectNotInClass = new(
        "Class.SubjectNotFound",
        "Sorry this subject is not found in that class");
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Instructors;
public static class InstructorErrors
{
    public static readonly Error InvalidRating = new Error(
        "Instructor.InvalidRating",
        "Invalid Value, Rating should be at least zero and at most 5");

    public static readonly Error AssignmentNotExist = new Error(
        "Instructor.InvalidAssignment",
        "Sorry this instructor is not assigned for this subject");
}

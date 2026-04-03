using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Reviews;
public sealed class InstructorsReview
{
    private InstructorsReview() { }
    public InstructorsReview(
        string instructorId,
        string studentId,
        Review review)
    {
        InstructorId = instructorId;
        StudentId = studentId;
        Review = review;
    }
    public string InstructorId { get; private set; }
    public string StudentId { get; private set; }
    public Review Review { get; private set; }

    public void UpdateReview(Review review)
    {
        if (review is null ||
            string.IsNullOrWhiteSpace(review.Value))
        {
            throw new ApplicationException("Sorry review is invalid");
        }

        Review = review;
    }

    public bool EnsureCanBeMutated(string studentId)
    {
        if (StudentId != studentId)
        {
            return false;
        }

        return true;
    }
}

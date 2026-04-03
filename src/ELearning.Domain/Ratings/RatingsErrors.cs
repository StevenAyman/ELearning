using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Ratings;
public static class RatingsErrors
{
    public static readonly Error InstructorNotTeaching = new(
        "Ratings.InstructorStudentNotMatch",
        "Sorry you can't rate instructor you're not buying his sessions");

    public static readonly Error NotRatingFound = new(
        "Ratins.NotRatingFound",
        "Sorry there's no rating found to cancel");

}

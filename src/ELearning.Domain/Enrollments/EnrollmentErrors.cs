using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Enrollments;
public static class EnrollmentErrors
{
    public static readonly Error SessionExpired = new(
        "Enrollment.Expired",
        "Sorry you can't access expired session");

    public static readonly Error VideoNotBelong = new(
        "Enrollment.VideoNotBelong",
        "Sorry you can't access this video because it doesn't belong to this session");

    public static readonly Error VideoNotUnlocked = new(
        "Enrollment.VideoNotUnlocked",
        "Sorry you can't access this video because it has a prerequisite finish it first");

    public static readonly Error VideoReachLimit = new(
        "Enrollment.VideoExceedLimit",
        "Sorry you can't access this video because you reached to the view limit");
}

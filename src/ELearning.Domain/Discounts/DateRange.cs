using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Discounts;
public sealed record DateRange
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int LengthInDays => !StartDate.HasValue || !EndDate.HasValue? 0 : EndDate.Value.Day - StartDate.Value.Day;
    public int LengthInHours => !StartDate.HasValue || !EndDate.HasValue ? 0 : EndDate.Value.Hour - StartDate.Value.Hour;
    private DateRange() { }
    private DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new ApplicationException("Start date shouldn't be in the future.");
        }

        return new DateRange(startDate, endDate);
    }
}

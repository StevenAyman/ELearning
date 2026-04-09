using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Exams;
public class ExamEnrollment : BaseEntity
{
    private ExamEnrollment() { }

    public ExamEnrollment(
        string id, 
        string purchaseId,
        string studentId,
        string examId, 
        int durationInSeconds, 
        int totalGrade) : base(id)
    {
        ExamId = examId;
        DurationInSeconds = durationInSeconds;
        TotalGrade = totalGrade;
        PurchaseId = purchaseId;
        StudentId = studentId;
    }

    public string PurchaseId { get; private set; }
    public string StudentId { get; private set; }
    public string ExamId { get; private set; }
    public int DurationInSeconds { get; private set; }
    public DateTime? EnrolledOnUtc { get; private set; }
    public DateTime? FinishedAtUtc { get; private set; }
    public double? Grade { get; private set; }
    public int TotalGrade { get; private set; }
    

    public void UpdateEnrollDate(DateTime enrollmentDate)
    {
        if (enrollmentDate > DateTime.UtcNow)
        {
            throw new ApplicationException("Sorry enrollment date can't be in the future");
        }
        EnrolledOnUtc = enrollmentDate;
    }

    public void UpdateEnrollDetails(double grade, DateTime finishTime)
    {
        if (grade < 0)
        {
            throw new ApplicationException("Sorry grade can't be below zero");
        }
        if (finishTime > DateTime.UtcNow)
        {
            throw new ApplicationException("Finish time can't be in the future");
        }
        Grade = grade;
        FinishedAtUtc = finishTime;
    }

}


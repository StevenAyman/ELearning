using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Exams;
public sealed class ExamEnrollmentDomainService
{
    public void EnrollOnExam(
        ExamEnrollment studentExam,
        Exam exam,
        DateTime utcNow)
    {
        if (studentExam is null)
        {
            throw new ApplicationException("Error you're enrolled in this exam");
        }

        if (exam is null)
        {
            throw new ApplicationException("Sorry this exam is not found");
        }

        if (studentExam.EnrolledOnUtc.HasValue)
        {
            throw new ApplicationException("Sorry you have enrolled at this exam before");
        }

        if (exam.Questions is null)
        {
            throw new ApplicationException("Sorry we can't find questions of this exam");
        }

        studentExam.UpdateEnrollDate(utcNow);
    }

    public void SubmitExam(
        ExamEnrollment studentExam,
        Exam exam,
        IEnumerable<ExamQuestionAnswer> studentAnswers,
        DateTime finishTime)
    {
        // 1. Checking for exam duration
        if (studentExam is null)
        {
            throw new ApplicationException("Can't find that you have purchased this exam");
        }

        if (!studentExam.EnrolledOnUtc.HasValue)
        {
            throw new ApplicationException("Sorry you didn't enroll at this exam before");
        }

        if (finishTime > studentExam.EnrolledOnUtc.Value.AddSeconds(studentExam.DurationInSeconds + 5))
        {
            throw new ApplicationException("You didn't finish at the required time");
        }

        // 2. Questions and answer from user
        if (studentAnswers is null)
        {
            throw new ApplicationException("Sorry student answers can't be null");
        }
        studentAnswers = studentAnswers.ToList();
        if (!studentAnswers.Any())
        {
            throw new ApplicationException("Student answers can't be empty");
        }

        if (exam.Questions is null)
        {
            throw new ApplicationException("Sorry this exam does not have questions");
        }

        if (exam.Questions.Count != studentAnswers.Count())
        {
            throw new ApplicationException("Sorry answers number doesn't match questions number");
        }

        var questions = exam.Questions.Where(q => q.Type == ExamQuestionType.Mcq).ToList();
        double score = 0;
        foreach(var answer in studentAnswers)
        {
            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question is null)
            {
                throw new ApplicationException("Sorry you provided an invalid question");
            }
            if (question.CorrectAnswer is null)
            {
                throw new ApplicationException("Correct answer shouldn't be null");
            }

            if (question.CorrectAnswer.Answer == answer.Answer)
            {
                score += question.Grade;
            }
            
        }

        studentExam.UpdateEnrollDetails(score, finishTime);
    }
}

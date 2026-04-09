using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Shared.Specifications.ExamQuestionSpecifications;
using ELearning.Domain.Shared.Specifications.QuizSpecifications;
using ELearning.Domain.Students;
using ELearning.Domain.Users;

namespace ELearning.Domain.Enrollments;
public class UserQuizDomainService()
{

    public UserQuiz CreateUserQuiz(
        Enrollment enrollment, 
        SessionQuiz sessionQuiz,
        string studentId,
        string userQuizId,
        DateTime utcNow)
    {
        // Check if quiz can be created

        if (enrollment is null)
        {
            throw new ApplicationException("You don't have access to this session");
        }

        if (!enrollment.HasQuiz)
        {
            throw new ApplicationException("This session doesn't contain any prerequisite quiz");
        }

        if (enrollment.StudentId != studentId)
        {
            throw new ApplicationException("Enrollment doesn't match this student");
        }

        if (enrollment.Status != EnrollmentStatus.Pending)
        {
            throw new ApplicationException("Can't create quiz for current state of session");
        }

        if (enrollment.RemainingTries == 0)
        {
            throw new ApplicationException("Sorry you can't take this quiz anymore you allowed tries are finished");
        }

        // Create NEW user quiz
        var newQuiz = new UserQuiz(userQuizId, enrollment.Id, sessionQuiz.Id, sessionQuiz.TotalGrades, sessionQuiz.PassingPercentage, sessionQuiz.DurationInSeconds, utcNow);

        // Get All questions related to this quiz.
        if (!sessionQuiz.Questions.Any())
        {
            throw new ApplicationException("Quiz doesn't contain any question so we can't make this quiz");
        }
        //enrollment.RemainingTries -= 1;

        // Get
        return newQuiz;
    }

    public double SendUserQuizResult(
        UserQuiz userQuiz, 
        int finishTime, 
        SessionQuiz sessionQuiz,
        IEnumerable<ExamQuestionAnswer> questionsWithAnswers, 
        DateTime utcNow)
    {

        if (userQuiz is null)
        {
            throw new ApplicationException("Invalid id user quiz is not found");
        }

        if (utcNow.AddSeconds(finishTime) > userQuiz.ExpiresAt)
        {
            throw new ApplicationException("Can't process this quiz because finish time is exceed expire time");
        }

        if (sessionQuiz is null)
        {
            throw new ApplicationException("Invalid quiz id");
        }

        if (!questionsWithAnswers.Any())
        {
            throw new ApplicationException("Invalid questions you question with user answers should be provided");
        }

        userQuiz.UpdateFinishTime(finishTime);
        var score = CorrectStudentAnswers(sessionQuiz.Questions, questionsWithAnswers);
        userQuiz.UpdateScore(score);

        return score;

    }

    private double CorrectStudentAnswers(IEnumerable<ExamQuestion> quizQuestions, IEnumerable<ExamQuestionAnswer> studentAnswers)
    {
        studentAnswers = studentAnswers.ToList();
        quizQuestions = quizQuestions.ToList();
        if (quizQuestions.Count() != studentAnswers.Count())
        {
            throw new ApplicationException("Error. Not all questions are passed");
        }
        double sumScore = 0;
        foreach (var studentAnswer in studentAnswers)
        {
            var question = quizQuestions.FirstOrDefault(q => q.Id == studentAnswer.Id);
            if (question is null ||
                question.CorrectAnswer is null)
            {
                throw new ApplicationException("Invalid operation. There's a question doesn't belong to quiz");
            }
           
            if (question.CorrectAnswer.Answer == studentAnswer.Answer)
            {
                sumScore += question.Grade;
            }
        }

        return sumScore;
    }

}

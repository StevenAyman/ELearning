using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Sessions;
public sealed class VideoQuestion : BaseEntity
{
    private VideoQuestion() { }
    public VideoQuestion(
        string id,
        string studentId,
        string videoId,
        string sessionId,
        string question) : base(id)
    {
        StudentId = studentId;
        VideoId = videoId;
        SessionId = sessionId;
        Question = question;
        Status = QuestionStatus.Open;
    }

    public string StudentId { get; private set; }
    public string VideoId { get; private set; }
    public string SessionId { get; private set; }
    public string Question { get; private set; }
    public string? Answer { get; private set; }
    public string? AssistantId { get; private set; }
    public QuestionStatus Status { get; private set; }
    public int Vote { get; private set; }

    public void DeleteQuestion()
    {
        if (Status == QuestionStatus.Deleted)
        {
            throw new ApplicationException("Sorry you can't delete deleted questions");
        }

        Status = QuestionStatus.Deleted;
    }

    public void AnswerQuestion(string answer, string assistantId)
    {
        if (Status == QuestionStatus.Answered)
        {
            throw new ApplicationException("Sorry can't answer this question. It's already answered");
        }

        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new ApplicationException("Answer can't be null or empty");
        }
        if (string.IsNullOrWhiteSpace(assistantId))
        {
            throw new ApplicationException("Invalid assistant id");
        }
        Answer = answer;
        Status = QuestionStatus.Answered;
        AssistantId = assistantId;
    }

    public void MarkQuestionAsClosed()
    {
        if (Status == QuestionStatus.Closed)
        {
            throw new ApplicationException("Question is already closed");
        }

        if (Status == QuestionStatus.Answered)
        {
            throw new ApplicationException("Sorry can't close answered question");
        }
    }

    public void EditQuestion(string newQuestion)
    {
        if (string.IsNullOrWhiteSpace(newQuestion))
        {
            throw new ApplicationException("Invalid question");
        }

        if (Status != QuestionStatus.Open)
        {
            throw new ApplicationException("Question should be open to edit");
        }

        Question = newQuestion;
    }

    public void EditAnswer(string newAnswer)
    {
        if (string.IsNullOrWhiteSpace(newAnswer))
        {
            throw new ApplicationException("Invalid answer");
        }

        if (Status != QuestionStatus.Answered)
        {
            throw new ApplicationException("Sorry can't edit answer where question is not in a valid state");
        }

        Answer = newAnswer;
    }
    
}

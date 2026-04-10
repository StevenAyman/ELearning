using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions.Events;
using ELearning.Domain.Shared;
using Microsoft.VisualBasic;

namespace ELearning.Domain.Sessions;
public sealed class QuestionAnswerVote : BaseEntity
{
    private QuestionAnswerVote() { }

    private QuestionAnswerVote(
        string id,
        string videoId,
        string questionId,
        string studentId) : base(id)
    {
        VideoId = videoId;
        QuestionId = questionId;
        StudentId = studentId;
    }

    public string VideoId { get; private set; }
    public string QuestionId { get; private set; }
    public string StudentId { get; private set; }

    public static QuestionAnswerVote Create(
        string id,
        string videoId,
        string quesitonId,
        string studentId)
    {
        var question = new QuestionAnswerVote(id, videoId, quesitonId, studentId);
        question.RaiseDomainEvent(new QuestionVotedDomainEvent(studentId, quesitonId, videoId));

        return question;
    }

}

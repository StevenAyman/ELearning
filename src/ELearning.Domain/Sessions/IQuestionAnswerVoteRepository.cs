using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Sessions;
public interface IQuestionAnswerVoteRepository
{
    Task<QuestionAnswerVote?> GetWithSpecAsync(IBaseSpecifications<QuestionAnswerVote> spec, CancellationToken cancellationToken = default);
    void Add(QuestionAnswerVote questionAnswerVote);
    void Update(QuestionAnswerVote questionAnswerVote);
    void Delete(QuestionAnswerVote questionAnswerVote);
}

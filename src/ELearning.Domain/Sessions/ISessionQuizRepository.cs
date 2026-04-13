using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Sessions;
public interface ISessionQuizRepository
{
    Task<SessionQuiz?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<SessionQuiz?> GetWithSpecAsync(IBaseSpecifications<SessionQuiz> spec, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SessionQuiz>> GetAllWithSpecAsync(IBaseSpecifications<SessionQuiz> spec, CancellationToken cancellationToken = default);
    void Add(SessionQuiz quiz);
    void Update(SessionQuiz quiz);
    void Delete(SessionQuiz quiz);
}

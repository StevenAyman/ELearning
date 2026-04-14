using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Enrollments;
public interface IUserQuizRepository
{
    Task<UserQuiz?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<UserQuiz?> GetWithSpecAsync(IBaseSpecifications<UserQuiz> spec, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserQuiz>> GetAllWithSpecAsync(IBaseSpecifications<UserQuiz> spec, CancellationToken cancellationToken = default);
    void Add(UserQuiz userQuiz);
}

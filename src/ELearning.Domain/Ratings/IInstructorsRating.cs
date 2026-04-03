using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Ratings;
public interface IInstructorsRating
{
    Task<bool> ExistsAsync(string sessionId, string studentId, CancellationToken cancellationToken = default);
    Task<InstructorsRating> GetWithSpecAsync(IBaseSpecifications<InstructorsRating> specs, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InstructorsRating>> GetAllWithSpecAsync(IBaseSpecifications<InstructorsRating> specs, CancellationToken cancellationToken = default);
    void Add(InstructorsRating instructorsRatings);
    void Update(InstructorsRating instructorsRatings);
    void Delete(InstructorsRating instructorsRatings);
}

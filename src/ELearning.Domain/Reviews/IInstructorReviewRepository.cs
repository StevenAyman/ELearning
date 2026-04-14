using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Reviews;
public interface IInstructorReviewRepository
{
    Task<InstructorsReview?> GetWithSpecAsync(IBaseSpecifications<InstructorsReview> spec, CancellationToken cancellationToken = default);
    void Add(InstructorsReview instructorsReview);
    void Update(InstructorsReview instructorsReview);
    void Delete(InstructorsReview instructorsReview);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Sessions;
public interface IVideoQuestionRepository
{
    Task<VideoQuestion?> GetWithSpecAsync(IBaseSpecifications<VideoQuestion> spec, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VideoQuestion>> GetAllWithSpecAsync(IBaseSpecifications<VideoQuestion> spec, CancellationToken cancellationToken = default);
    void Add(VideoQuestion videoQuestion);
    void Update(VideoQuestion videoQuestion);
    void Delete(VideoQuestion videoQuestion);
}

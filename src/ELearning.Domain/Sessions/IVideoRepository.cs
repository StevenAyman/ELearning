using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Sessions;
public interface IVideoRepository
{
    Task<Video?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Video?> GetWithSpecAsync(IBaseSpecifications<Video> specs, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Video>> GetAllWithSpecAsync(IBaseSpecifications<Video> specs, CancellationToken cancellationToken = default);

    void Add(Video video);
    void Update(Video video);
    void Delete(Video video);

}

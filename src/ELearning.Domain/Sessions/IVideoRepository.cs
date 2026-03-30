using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Sessions;
public interface IVideoRepository
{
    Task<Video> GetById(string id);
    Task<IQueryable<Video>> GetBySessionId(string sessionId);

}

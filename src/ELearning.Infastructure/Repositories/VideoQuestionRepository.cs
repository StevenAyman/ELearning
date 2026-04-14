using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;

namespace ELearning.Infastructure.Repositories;
public sealed class VideoQuestionRepository : Repository<VideoQuestion>, IVideoQuestionRepository
{
    public VideoQuestionRepository(AppDbContext dbContext) : base(dbContext)
    {  
    }
}

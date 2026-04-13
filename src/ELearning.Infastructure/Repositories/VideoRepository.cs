using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;

namespace ELearning.Infastructure.Repositories;
public sealed class VideoRepository : Repository<Video>, IVideoRepository
{
    public VideoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

}

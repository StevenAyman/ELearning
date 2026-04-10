using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Assistants;
public sealed class Assistant : BaseEntity
{
    private Assistant() { }
    public Assistant(
        string id,
        string instructorId) : base(id)
    {
        InstructorId = instructorId;
    }

    public string InstructorId { get; private set; }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared;

public abstract class BaseEntity
{
    protected BaseEntity() { }
    protected BaseEntity(string id)
    {
        Id = id;
    }
    public string Id { get; private set; }
}

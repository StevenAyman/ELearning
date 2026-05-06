using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Classes;
public sealed class LearningClass
{
    private LearningClass() { }
    public LearningClass(string id, TypeName type)
    {
        Id = id;
        Type = type;
    }

    public string Id { get; private set; }
    public TypeName Type { get; private set; }

    public void UpdateType(TypeName newType)
    {
        Type = newType;
    }
}

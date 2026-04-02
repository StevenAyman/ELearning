using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Subjects;
public sealed class Subject : BaseEntity
{
    private Subject() { }
    public Subject(
        string id,
        TypeName name) : base(id)
    {
        Name = name;
    }

    public TypeName Name { get; private set; }

}

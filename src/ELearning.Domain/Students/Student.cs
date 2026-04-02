using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Students;
public sealed class Student : BaseEntity
{
    private Student() { }

    public Student(string id) : base(id)
    {
        Wallet = Money.Zero();
    }

    public Money Wallet { get; private set; }
}

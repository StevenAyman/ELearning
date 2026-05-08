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

    public Student(string id, string? classId) : base(id)
    {
        Wallet = Money.Zero();
        ClassId = classId;
    }

    public Money Wallet { get; private set; }
    public string? ClassId { get; private set; }

    public void UpdateClassId(string classId)
    {
        ClassId = classId;
    }

    public void Withdraw(Money amountToPay)
    {
        if (Wallet - amountToPay < Money.Zero())
        {
            throw new ApplicationException("There's not sufficient amount of money in wallet");
        }

        Wallet -= amountToPay;
    }

    public void Deposite(Money amountToDeposite)
    {
        Wallet += amountToDeposite;
    }
}

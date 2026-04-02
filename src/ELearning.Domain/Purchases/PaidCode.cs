using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Purchases;
public sealed class PaidCode : BaseEntity
{
    private PaidCode() { }
    public PaidCode(
        string id,
        string code,
        Money balance, 
        DateTime generatedAtUtc) : base(id)
    {
        Balance = balance;
        GeneratedAtUtc = generatedAtUtc;
        Code = code;
    }
    public string Code { get; private set; }
    public Money Balance { get; private set; }
    public PaidCodeStatus Status {  get; private set; }
    public string? StudentId { get; private set; }
    public DateTime GeneratedAtUtc { get; private set; }
    public DateTime? UsedAtUtc { get; private set; }
    public DateTime? ExpiredAtUtc { get; private set; }

    public void ExpireCode(DateTime utcNow)
    {

        ValidateCodeStatus();
        Status = PaidCodeStatus.Expired;
        ExpiredAtUtc = utcNow;
    }

    public void UseCode(string studentId, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ApplicationException("Error. Student id can't be null");
        }

        ValidateCodeStatus();

        StudentId = studentId;
        Status = PaidCodeStatus.Used;
        UsedAtUtc = utcNow;
    }

    private void ValidateCodeStatus()
    {
        if (Status == PaidCodeStatus.Expired)
        {
            throw new ApplicationException("Error. This code is already expired.");
        }

        if (Status == PaidCodeStatus.Used)
        {
            throw new ApplicationException("Error. This code is used before. you can't expire it.");
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Purchases.Events;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Purchases;
public sealed class Purchase : BaseEntity
{
    private Purchase() { }

    public Purchase(
        string id,
        string studentId,
        DateTime utcNow,
        string? sessionId = null,
        string? examId = null) : base(id)
    {  
        StudentId = studentId;
        SessionId = sessionId;
        ExamId = examId;
        CreatedAtUtc = utcNow;
        Status = PaymentStatus.Pending;
    }

    public string StudentId { get; private set; }
    public string? SessionId { get; private set; }
    public string? ExamId { get; private set; }
    public PurchaseMethod? PaymentMethod { get; private set; }
    public Money TotalPaid { get; private set; }
    public string? CodeId { get; private set; } // Discount Code if available
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? PurchasedAtUtc { get; private set; }
    public DateTime? RefundedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    
    public void FailPurchase(DateTime utcNow)
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new ApplicationException("The purchase is already fulfilled");
        }

        Status = PaymentStatus.Failed;
        RaiseDomainEvent(new PurchaseFailedDomainEvent(Id));
        CompletedAtUtc = utcNow;
    }

    public void SuccessPurchase(DateTime utcNow)
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new ApplicationException("The purchase is already fulfilled");
        }

        Status = PaymentStatus.Successeded;
        RaiseDomainEvent(new PurchaseSuccessededDomainEvent(Id));
        PurchasedAtUtc = utcNow;
        CompletedAtUtc = utcNow;

    }

    public void Checkout(PurchaseMethod purchaseMethod, Money paidAmount, DiscountCode? code = null)
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new ApplicationException("Can't process a processed purchase");
        }

        if (purchaseMethod is null)
        {
            throw new ApplicationException("Purchase method is not valid");
        }

        if (paidAmount is null || paidAmount <= Money.Zero())
        {
            throw new ApplicationException("Invalid amount to pay");
        }

        if (code is not null)
        {
            CodeId = code.Id;
        }

        TotalPaid = paidAmount;
        PaymentMethod = purchaseMethod;
    }

    public static Purchase CreateSessionPurchase(string id, string studentId, string sessionId, DateTime utcNow)
    {
        return new Purchase(id, studentId, utcNow, sessionId);
    }

    public static Purchase CreateExamPurchase(string id, string studentId, string examId, DateTime utcNow)
    {
        return new Purchase(id, studentId, utcNow, null, examId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Purchases;
using ELearning.Domain.Purchases.Events;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Students;
public sealed class PaymentDomainService
{
    public readonly Money ToPercentage = new Money(1/100m);
    public Purchase CreateSessionPayment(
        string id, 
        Student student, 
        Session session, 
        IReadOnlyList<Purchase> studentPurchases,
        DateTime utcNow)
    {
        if (session is null)
        {
            throw new ApplicationException("Session shouldn't be null");
        }

        if (student is null)
        {
            throw new ApplicationException("Student shouldn't be null");
        }

        if (studentPurchases is null)
        {
            throw new ApplicationException("Student purchases shouldn't be null");
        }

        var alreadyPurchased = studentPurchases.FirstOrDefault(p => p.SessionId == session.Id);
        if (alreadyPurchased is not null)
        {
            throw new ApplicationException("Session already purchased");
        }

        var purchase = Purchase.CreateSessionPurchase(id, student.Id, session.Id, utcNow);
        purchase.RaiseDomainEvent(new PurchaseCreatedDomainEvent(purchase.Id));

        return purchase;
    }

    private Money CalculateDiscountedAmount(Session session, DiscountCode? discountCode)
    {
        var discountAmount = Money.Zero();
        var sessionPrice = session.Price;
        if (discountCode is null)
        {
            return discountAmount;
        }

        if (discountCode.ExpiredAtUtc is not null)
        {
            throw new ApplicationException("The discount code is expired can't be applied");
        }

        discountAmount = discountCode.DiscountType switch
        {
            DiscountAmountType.FixedAmount => discountCode.DiscountAmount,
            DiscountAmountType.Percentage => sessionPrice * ( discountCode.DiscountAmount * ToPercentage),
            _ => Money.Zero()
        };


        return discountAmount;
    }

    public Money CalculateTotalPurchaseAmount(Session session, DiscountCode? discountCode)
    {
        if (session is null)
        {
            throw new ApplicationException("Session can't be null or not defined");
        }

        var discountAmount = discountCode is null ? Money.Zero() : CalculateDiscountedAmount(session, discountCode);
        
        var totalPrice = session.Price - discountAmount;

        if (totalPrice < Money.Zero())
        {
            throw new ApplicationException("This discount is not can't be applied.");
        }

        return totalPrice;
    }
}

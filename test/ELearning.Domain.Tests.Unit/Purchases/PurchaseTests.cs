using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Purchases;
using ELearning.Domain.Purchases.Events;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Purchases;
public sealed class PurchaseTests
{
    private readonly Purchase _sut;

    public PurchaseTests()
    {
        _sut = Purchase.CreateSessionPurchase("id", "12", "123", DateTime.UtcNow);
    }

    [Fact]
    public void FailPurchase_ShouldMarkPurchaseAsFailed_WhenItIsFailed()
    {
        // Arrange
        var utc = DateTime.UtcNow;

        // Act
        _sut.FailPurchase(utc);

        // Assert
        _sut.Status.Should().Be(PaymentStatus.Failed);
        _sut.CompletedAtUtc.Should().Be(utc);
        _sut.PurchasedAtUtc.Should().Be(null);
        _sut.RefundedAtUtc.Should().Be(null);
    }

    [Fact]
    public void FailPurchase_ShouldRaiseDomainEvent_WhenPurchaseMarkedAsFailed()
    {
        // Arrange
        var utc = DateTime.UtcNow;

        // Act
        _sut.FailPurchase(utc);

        // Assert
        _sut.Status.Should().Be(PaymentStatus.Failed);
        _sut.DomainEvents.Should().HaveCount(1);
        _sut.DomainEvents.Should().Contain(new PurchaseFailedDomainEvent(_sut.Id));
    }

    [Fact]
    public void FailPurchase_ShouldThrowException_WhenPurchaseStatusNotPending()
    {
        // Arrange
        var utc = DateTime.UtcNow;
        _sut.SuccessPurchase(utc);

        // Act
        Action result = () => _sut.FailPurchase(utc);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The purchase is already fulfilled");
        
    }

    [Fact]
    public void SuccessPurchase_ShouldMarkPurchaseAsSuccesseded_WhenItIsCompletedSuccessfully()
    {
        // Arrange
        var utc = DateTime.UtcNow;

        // Act
        _sut.SuccessPurchase(utc);

        // Assert
        _sut.Status.Should().Be(PaymentStatus.Successeded);
        _sut.CompletedAtUtc.Should().Be(utc);
        _sut.PurchasedAtUtc.Should().Be(utc);
        _sut.RefundedAtUtc.Should().Be(null);
    }

    [Fact]
    public void SuccessPurchase_ShouldRaiseDomainEvent_WhenItIsCompletedSuccessfully()
    {
        // Arrange
        var utc = DateTime.UtcNow;

        // Act
        _sut.SuccessPurchase(utc);

        // Assert
        _sut.Status.Should().Be(PaymentStatus.Successeded);
        _sut.DomainEvents.Should().HaveCount(1);
        _sut.DomainEvents.Should().Contain(new PurchaseSuccessededDomainEvent(_sut.Id));
    }

    [Fact]
    public void SuccessPurchase_ShouldThrowException_WhenPurchaseStatusNotPending()
    {
        // Arrange
        var utc = DateTime.UtcNow;
        _sut.FailPurchase(utc);

        // Act
        Action result = () => _sut.SuccessPurchase(utc);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The purchase is already fulfilled");

    }

    [Fact]
    public void Checkout_ShouldSetPurchaseFinalPriceAndPurchaseMethod_WhenInputsAreValid()
    {
        // Arrange
        var purchaseMethod = new PurchaseMethod("12", PaymentType.Wallet);
        var paidMoney = new Money(50);

        // Act
        _sut.Checkout(purchaseMethod, paidMoney);

        // Assert
        _sut.PaymentMethod.Should().BeEquivalentTo(purchaseMethod);
        _sut.TotalPaid.Should().Be(paidMoney);
        _sut.CodeId.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Checkout_ShouldThrowException_WhenPurchaseStatusIsNotPending()
    {
        // Arrange
        var purchaseMethod = new PurchaseMethod("12", PaymentType.Wallet);
        var paidMoney = new Money(50);
        _sut.FailPurchase(DateTime.UtcNow);

        // Act
        Action result = () => _sut.Checkout(purchaseMethod, paidMoney);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Can't process a processed purchase");
    }

    [Fact]
    public void Checkout_ShouldThrowException_WhenPurchaseMethodIsNotGiven()
    {
        // Arrange
        var paidMoney = new Money(50);

        // Act
        Action result = () => _sut.Checkout(null!, paidMoney);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Purchase method is not valid");
    }

    [Fact]
    public void Checkout_ShouldThrowException_WhenPaidAmountNotValid()
    {
        // Arrange
        var purchaseMethod = new PurchaseMethod("12", PaymentType.Wallet);

        // Act
        Action result = () => _sut.Checkout(purchaseMethod, null!);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Invalid amount to pay");
    }
}

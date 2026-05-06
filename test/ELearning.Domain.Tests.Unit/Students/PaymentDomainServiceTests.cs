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
using ELearning.Domain.Students;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Students;
public sealed class PaymentDomainServiceTests
{
    private readonly PaymentDomainService _sut;
    public PaymentDomainServiceTests()
    {
        _sut = new PaymentDomainService();
    }

    [Fact]
    public void CreateSessionPayment_ShouldCreateSessionPayment_WhenDataIsValid()
    {
        // Arrange
        var id = $"p_{Guid.CreateVersion7()}";
        var student = new Student($"", "");
        var session = new Session(
            "",
            new Title(""), 
            new Description(""),
            new Money(60), 
            SessionStatus.Draft, 
            DateTime.UtcNow, 
            "", 
            "",
            "");
        var purchasedSessions = new List<Purchase>();
        var utcNow = DateTime.UtcNow;
        var expected = Purchase.CreateSessionPurchase(id, "", "", utcNow);

        // Act
        var result = _sut.CreateSessionPayment(id, student, session, purchasedSessions, utcNow);

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.Excluding(o => o.DomainEvents));

    }

    [Fact]
    public void CreateSessionPayment_ShouldThrowException_WhenSessionIsNull()
    {
        // Arrange
        var id = $"p_{Guid.CreateVersion7()}";
        var student = new Student($"", "");
        var purchasedSessions = new List<Purchase>();
        var utcNow = DateTime.UtcNow;

        // Act
        Action result = () => _sut.CreateSessionPayment(id, student, null!, purchasedSessions, utcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Session shouldn't be null");

    }

    [Fact]
    public void CreateSessionPayment_ShouldThrowException_WhenStudentIsNull()
    {
        // Arrange
        var id = $"p_{Guid.CreateVersion7()}";
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var purchasedSessions = new List<Purchase>();
        var utcNow = DateTime.UtcNow;

        // Act
        Action result = () => _sut.CreateSessionPayment(id, null!, session, purchasedSessions, utcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Student shouldn't be null");

    }

    [Fact]
    public void CreateSessionPayment_ShouldThrowException_WhenStudentPurchasesAreNull()
    {
        // Arrange
        var id = $"p_{Guid.CreateVersion7()}";
        var student = new Student($"", "");
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var utcNow = DateTime.UtcNow;

        // Act
        Action result = () => _sut.CreateSessionPayment(id, student, session, null!, utcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Student purchases shouldn't be null");

    }

    [Fact]
    public void CreateSessionPayment_ShouldThrowException_WhenSessionIsAlreadyPurchased()
    {
        // Arrange
        var id = $"p_{Guid.CreateVersion7()}";
        var student = new Student($"", "");
        var session = new Session(
        "1",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var utcNow = DateTime.UtcNow;
        var studentPurchases = new List<Purchase>() { Purchase.CreateSessionPurchase("", "", "1", utcNow) };

        // Act
        Action result = () => _sut.CreateSessionPayment(id, student, session, studentPurchases, utcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Session already purchased");

    }

    [Fact]
    public void CreateSessionPayment_ShouldRaiseDomainEvent_WhenPurchaseIsCreatedSuccessfully()
    {
        // Arrange
        var id = $"p_{Guid.CreateVersion7()}";
        var student = new Student($"", "");
        var session = new Session(
            "",
            new Title(""),
            new Description(""),
            new Money(60),
            SessionStatus.Draft,
            DateTime.UtcNow,
            "",
            "", "");
        var purchasedSessions = new List<Purchase>();
        var utcNow = DateTime.UtcNow;

        // Act
        var result = _sut.CreateSessionPayment(id, student, session, purchasedSessions, utcNow);

        // Assert
        result.DomainEvents.Should().HaveCount(1);
        result.DomainEvents.Should().ContainEquivalentOf(new PurchaseCreatedDomainEvent(id));

    }

    [Fact]
    public void CalculateTotalPurchaseAmount_ShouldDiscountAmountBeZero_WhenTheresNoDiscountCodeGiven()
    {
        // Arrange
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");

        // Act
        var result = _sut.CalculateTotalPurchaseAmount(session, null);

        // Assert
        result.Amount.Should().BeGreaterThan(0m);
        result.Should().Be(session.Price);
    }

    [Fact]
    public void CalculateTotalPurchaseAmount_ShouldThrowException_WhenSessionIsNull()
    {
        // Arrange


        // Act
        Action result = () => _sut.CalculateTotalPurchaseAmount(null!, null);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Session can't be null or not defined");
    }

    [Fact]
    public void CalculateTotalPurchaseAmount_ShouldThrowException_WhenDiscountCodeIsExpired()
    {
        // Arrange
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("Anything")
            .WithCode("Anything")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(50))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);

        discountCode.ExpireCode(DateTime.UtcNow);

        // Act
        Action result = () => _sut.CalculateTotalPurchaseAmount(session, discountCode);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The discount code is expired can't be applied");
    }

    [Fact]
    public void CalculateTotalPurchaseAmount_ShouldCalculateDiscountAmountCorrectly_WhenDiscountAmountTypeIsFixed()
    {
        // Arrange
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var discountAmount = new Money(50);
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("Anything")
            .WithCode("Anything")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(discountAmount)
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);

        // Act
        var result = _sut.CalculateTotalPurchaseAmount(session, discountCode);

        // Assert
        result.Amount.Should().BeGreaterThan(0m);
        result.Should().Be(session.Price - discountAmount);
    }

    [Fact]
    public void CalculateTotalPurchaseAmount_ShouldCalculateDiscountAmountCorrectly_WhenDiscountAmountTypeIsPercentage()
    {
        // Arrange
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var discountAmount = new Money(0.5m);
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("Anything")
            .WithCode("Anything")
            .WithDiscountAmountType(DiscountAmountType.Percentage)
            .WithDiscountAmount(discountAmount)
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);
    
        // Act
        var result = _sut.CalculateTotalPurchaseAmount(session, discountCode);

        // Assert
        result.Amount.Should().BeGreaterThan(0m);
        result.Should().Be(session.Price - session.Price * discountAmount * _sut.ToPercentage);
    }

    [Fact]
    public void CalculateTotalPurchaseAmount_ShouldThrowException_WhenDiscountAmountIsMoreThanTheSessionPrice()
    {
        // Arrange
        var session = new Session(
        "",
        new Title(""),
        new Description(""),
        new Money(60),
        SessionStatus.Draft,
        DateTime.UtcNow,
        "",
        "", "");
        var discountAmount = new Money(70);
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("Anything")
            .WithCode("Anything")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(discountAmount)
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);

        // Act
        Action result = () => _sut.CalculateTotalPurchaseAmount(session, discountCode);

        // Assert
        result.Should().Throw<ArgumentException>();
    }
}

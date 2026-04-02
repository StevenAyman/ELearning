using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Discounts.Events;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Discounts;
public sealed class DiscountTests
{
    private readonly DiscountCode _sut;

    public DiscountTests()
    {
        _sut = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);
    }

    [Fact]
    public void ExpireCode_ShouldReturnFailureResult_WhenCodeIsAlreadyExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Failure(DiscountErrors.ExpiredCode);
        _sut.ExpireCode(utcNow);

        // Act
        var result = _sut.ExpireCode(utcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ExpireCode_ShouldReturnSuccessResult_WhenCodeCanBeExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Success();

        // Act
        var result = _sut.ExpireCode(utcNow);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        _sut.ExpiredAtUtc.Should().Be(utcNow);
    }

    [Fact]
    public void UseCode_ShouldReturnFailureResult_WhenCodeIsExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Failure(DiscountErrors.ExpiredCode);
        _sut.ExpireCode(utcNow);

        // Act
        var result = _sut.UseCode(utcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UseCode_ShouldReturnSuccessResult_WhenCodeCanBeUsed()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Success();

        // Act
        var result = _sut.UseCode(utcNow);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        _sut.LastUsedAtUtc.Should().Be(utcNow);
        _sut.CurrentCount.Should().Be(1);
    }

    [Fact]
    public void UseCode_ShouldRaiseDomainEvent_WhenCodeIsUsed()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;

        // Act
        var result = _sut.UseCode(utcNow);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        _sut.DomainEvents.Should().HaveCount(1);
        _sut.DomainEvents.Should().Contain(new DiscountCodeUsedDomainEvent(_sut.Id));
    }

    [Fact]
    public void UpdateDiscountAmount_ShouldReturnFailureResult_WhenDiscountAmountIsNull()
    {
        // Arrange
        var expected = Result.Failure(DiscountErrors.InvalidDiscountAmount);
        // Act
        var result = _sut.UpdateDiscountAmount(null!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateDiscountAmount_ShouldReturnFailureResult_WhenDiscountCodeIsExpired()
    {
        // Arrange
        _sut.ExpireCode(DateTime.UtcNow);
        var expected = Result.Failure(DiscountErrors.ExpiredCode);

        // Act
        var result = _sut.UpdateDiscountAmount(new Money(20));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateDiscountAmount_ShouldReturnFailureResult_WhenDiscountTypeIsPercentageAndDiscountAmountIsInvalid()
    {
        // Arrange
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.Percentage)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);
        var expected = Result.Failure(DiscountErrors.InvalidPercentageAmount);

        // Act
        var result = discountCode.UpdateDiscountAmount(new Money(101));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateDiscountAmount_ShouldReturnFailureResult_WhenDiscountTypeIsFixedAndDiscountAmountIsInvalid()
    {
        // Arrange
        
        var expected = Result.Failure(DiscountErrors.InvalidDiscountAmount);

        // Act
        var result = _sut.UpdateDiscountAmount(new Money(0));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(45.7)]
    [InlineData(50)]
    public void UpdateDiscountAmount_ShouldReturnSuccessResultAndUpdateDiscountAmount_WhenDiscountTypeIsFixedAndDiscountAmountIsValid(decimal amount)
    {
        // Arrange
        var money = new Money(amount);
        var expected = Result.Success();

        // Act
        var result = _sut.UpdateDiscountAmount(money);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        _sut.DiscountAmount.Should().Be(money);
    }

    [Theory]
    [InlineData(1.5)]
    [InlineData(5)]
    [InlineData(0.5)]
    [InlineData(99.5)]
    [InlineData(85)]
    public void UpdateDiscountAmount_ShouldReturnSuccessResultAndUpdateDiscountAmount_WhenDiscountTypeIsPercentageAndDiscountAmountIsValid(decimal amount)
    {
        // Arrange
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.Percentage)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);
        var money = new Money(amount);
        var expected = Result.Success();

        // Act
        var result = discountCode.UpdateDiscountAmount(money);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        discountCode.DiscountAmount.Should().Be(money);
    }

    [Fact]
    public void UpdateCountLimit_ShouldReturnFailureResult_WhenCodeIsExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Failure(DiscountErrors.ExpiredCode);
        _sut.ExpireCode(utcNow);

        // Act
        var result = _sut.UpdateCountLimit(5);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateCountLimit_ShouldReturnFailureResult_WhenExpirationCriteriaIsNotLimitedCount()
    {
        // Arrange
        var expected = Result.Failure(DiscountErrors.InvalidExpirationCriteria);
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.Percentage)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, DateTimeKind.Utc), DateTime.UtcNow))
            .Build(DateTime.UtcNow);


        // Act
        var result = discountCode.UpdateCountLimit(5);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateCountLimit_ShouldReturnFailureResult_WhenUpdatedLimitCountIsLessThanOrEqualCurrentCount()
    {
        // Arrange
        var expected = Result.Failure(DiscountErrors.InvalidLimitCount);
        _sut.UseCode(DateTime.UtcNow);

        // Act
        var result = _sut.UpdateCountLimit(0);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateCountLimit_ShouldReturnSuccessResultAndUpdateCountLimit_WhenCodeIsApplicableToDoThat()
    {
        // Arrange
        var expected = Result.Success();

        // Act
        var result = _sut.UpdateCountLimit(10);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateDiscountCode_ShouldReturnFailureResult_WhenCodeIsExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Failure(DiscountErrors.ExpiredCode);
        _sut.ExpireCode(utcNow);

        // Act
        var result = _sut.UpdateDiscountCode("223232");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    public void UpdateDiscountCode_ShouldReturnFailureResult_WhenNewValueIsNullOrEmpty(string? code)
    {
        // Arrange
        var expected = Result.Failure(DiscountErrors.InvalidCode);

        // Act
        var result = _sut.UpdateDiscountCode(code!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateDiscountCode_ShouldReturnSuccessResultAndUpdateCode_WhenNewValueIsValid()
    {
        // Arrange
        var expected = Result.Success();
        var newValue = "123New";

        // Act
        var result = _sut.UpdateDiscountCode(newValue);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        _sut.Code.Should().NotBeNull();
        _sut.Code.Should().Be(newValue);
    }

    [Fact]
    public void UpdateExpirePeriod_ShouldReturnFailureResult_WhenCodeIsExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var expected = Result.Failure(DiscountErrors.ExpiredCode);
        var newPeriod = DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        _sut.ExpireCode(utcNow);

        // Act
        var result = _sut.UpdateExpirePeriod(newPeriod);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateExpirePeriod_ShouldReturnFailureResult_WhenExpirationCriteriaIsNotPeriod()
    {
        // Arrange
        var expected = Result.Failure(DiscountErrors.InvalidExpirationCriteria);
        var newPeriod = DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        // Act
        var result = _sut.UpdateExpirePeriod(newPeriod);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateExpirePeriod_ShouldReturnFailureResult_WhenExpirationPeriodNullOrEmpty()
    {
        // Arrange
        var expected = Result.Failure(DiscountErrors.InvalidExpirePeriod);
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
        .SetId("11")
        .WithCode("2222")
        .WithDiscountAmountType(DiscountAmountType.FixedAmount)
        .WithDiscountAmount(new Money(23))
        .WithExpirationType(DiscountExpirationType.Period)
        .WithExpirePeriod(DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)))
        .Build(DateTime.UtcNow);
        // Act
        var result = discountCode.UpdateExpirePeriod(null!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UpdateExpirePeriod_ShouldReturnSuccessResultAndUpdateExpirePeriod_WhenExpirationPeriodIsValid()
    {
        // Arrange
        var expected = Result.Success();
        var newPeriod = DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(5));
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)))
            .Build(DateTime.UtcNow);
        // Act
        var result = discountCode.UpdateExpirePeriod(newPeriod);

        // Assert
        result.IsSuccuss.Should().BeTrue();
        result.Should().BeEquivalentTo(expected);
        discountCode.ExpirePeriod.Should().NotBeNull();
        discountCode.ExpirePeriod.Should().BeEquivalentTo(newPeriod);
    }
}

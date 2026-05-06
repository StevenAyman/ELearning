using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Discounts;
public sealed class DiscountDomainServiceTests
{
    private readonly DiscountDomainService _sut;

    public DiscountDomainServiceTests()
    {
        _sut = new DiscountDomainService();
    }


    [Fact]
    public void ValidateDiscountCodeExpiration_ShouldReturnTrue_WhenDiscountCodeIsNotExpired()
    {
        // Arrange
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);

        // Act
        var result = _sut.ValidateDiscountCodeExpiration(discountCode, DateTime.UtcNow);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateDiscountCodeExpiration_ShouldThrowException_WhenDiscountCodeIsNull()
    {
        // Arrange

        // Act
        Action result = () => _sut.ValidateDiscountCodeExpiration(null!, DateTime.UtcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Invalid discount code");
    }

    [Fact]
    public void ValidateDiscountCodeExpiration_ShouldReturnFalse_WhenDiscountCodeIsExpired()
    {
        // Arrange
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(100)
            .Build(DateTime.UtcNow);
        discountCode.ExpireCode(DateTime.UtcNow);

        // Act
        var result = _sut.ValidateDiscountCodeExpiration(discountCode, DateTime.UtcNow);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ValidateDiscountCodeExpiration_ShouldReturnFalseAndExpireCode_WhenTheCurrentCountEqualToLimitAndExpirationPolicyIsLimitedCount()
    {
        // Arrange
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.LimitedCount)
            .WithCountLimit(2)
            .Build(DateTime.UtcNow);
        discountCode.UseCode(DateTime.UtcNow);
        discountCode.UseCode(DateTime.UtcNow);
        var utcNow = DateTime.UtcNow;
        // Act
        var result = _sut.ValidateDiscountCodeExpiration(discountCode, utcNow);

        // Assert
        result.Should().BeFalse();
        discountCode.ExpiredAtUtc.Should().NotBeNull();
        discountCode.ExpiredAtUtc.Should().Be(utcNow);
    }

    [Fact]
    public void ValidateDiscountCodeExpiration_ShouldReturnFalseAndExpireCode_WhenEndDateFinishedAndExpirationPolicyIsExpirationPeriod()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, 5, 1, 3, 3, 3, 3,DateTimeKind.Utc), utcNow))
            .Build(DateTime.UtcNow);
        discountCode.UseCode(DateTime.UtcNow);
        discountCode.UseCode(DateTime.UtcNow);
        // Act
        var result = _sut.ValidateDiscountCodeExpiration(discountCode, utcNow);

        // Assert
        result.Should().BeFalse();
        discountCode.ExpiredAtUtc.Should().NotBeNull();
        discountCode.ExpiredAtUtc.Should().Be(utcNow);
    }

    [Fact]
    public void IsDiscountCodeApplicable_ShouldThrowException_WhenGivenSessionIsNull()
    {
        // Arrange
        var codeApplicableAreas = new List<CodeApplicableArea>()
        {
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.General)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Instructor)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Subject)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Session)),
        };
        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, 5, 1, 3, 3, 3, 3, DateTimeKind.Utc), DateTime.UtcNow))
            .Build(DateTime.UtcNow);

        var codeAreas = new List<CodeAreas>()
        {
            new CodeAreas("11", 1, null)
        };

        // Act
        Action result = () => _sut.IsDiscountCodeApplicable(null!, discountCode, codeAreas, codeApplicableAreas);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Session can't be null");
    }

    [Fact]
    public void IsDiscountCodeApplicable_ShouldThrowException_WhenGivenCodeAreasIsNull()
    {
        // Arrange
        var codeApplicableAreas = new List<CodeApplicableArea>()
        {
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.General)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Instructor)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Subject)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Session)),
        };
        var session = new Session(
            "1", 
            new Title("Session 01"),
            new Description("dfd"),
            new Money(40), 
            SessionStatus.Draft, 
            DateTime.UtcNow,
            "2",
            "1", "");

        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, 5, 1, 3, 3, 3, 3, DateTimeKind.Utc), DateTime.UtcNow))
            .Build(DateTime.UtcNow);


        // Act
        Action result = () => _sut.IsDiscountCodeApplicable(session, discountCode, null!, codeApplicableAreas);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The code Areas shouldn't be null");
    }

    [Fact]
    public void IsDiscountCodeApplicable_ShouldThrowException_WhenGivenCodeApplicableAreaIsNull()
    {
        // Arrange

        var session = new Session(
            "1",
            new Title("Session 01"),
            new Description("dfd"),
            new Money(40),
            SessionStatus.Draft,
            DateTime.UtcNow,
            "2",
            "1", "");

        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, 5, 1, 3, 3, 3, 3, DateTimeKind.Utc), DateTime.UtcNow))
            .Build(DateTime.UtcNow);
        var codeAreas = new List<CodeAreas>()
        {
            new CodeAreas("11", 1, null)
        };

        // Act
        Action result = () => _sut.IsDiscountCodeApplicable(session, discountCode, codeAreas, null!);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The code Areas shouldn't be null");
    }

    [Fact]
    public void IsDiscountCodeApplicable_ShouldThrowException_WhenCodeAreasDoesntContainAValidDiscountApplicableArea()
    {
        // Arrange
        var codeApplicableAreas = new List<CodeApplicableArea>()
        {
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.General)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Instructor)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Subject)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Session)),
        };
        var session = new Session(
            "1",
            new Title("Session 01"),
            new Description("dfd"),
            new Money(40),
            SessionStatus.Draft,
            DateTime.UtcNow,
            "2",
            "1", "");

        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, 5, 1, 3, 3, 3, 3, DateTimeKind.Utc), DateTime.UtcNow))
            .Build(DateTime.UtcNow);
        var codeAreas = new List<CodeAreas>()
        {
            new CodeAreas("11", 20, null)
        };

        // Act
        Action result = () => _sut.IsDiscountCodeApplicable(session, discountCode, codeAreas, codeApplicableAreas);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Something went wrong.");
    }

    [Fact]
    public void IsDiscountCodeApplicable_ShouldThrowException_WhenCodeAreasDoesntContainAValidDiscountCode()
    {
        // Arrange
        var codeApplicableAreas = new List<CodeApplicableArea>()
        {
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.General)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Instructor)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Subject)),
            new CodeApplicableArea(new TypeName(DiscountApplicableAreas.Session)),
        };
        var session = new Session(
            "1",
            new Title("Session 01"),
            new Description("dfd"),
            new Money(40),
            SessionStatus.Draft,
            DateTime.UtcNow,
            "2",
            "1", "");

        var discountCode = DiscountCode.DiscountCodeBuilder.CreateBuilder()
            .SetId("11")
            .WithCode("2222")
            .WithDiscountAmountType(DiscountAmountType.FixedAmount)
            .WithDiscountAmount(new Money(23))
            .WithExpirationType(DiscountExpirationType.Period)
            .WithExpirePeriod(DateRange.Create(new DateTime(2002, 5, 1, 3, 3, 3, 3, DateTimeKind.Utc), DateTime.UtcNow))
            .Build(DateTime.UtcNow);
        var codeAreas = new List<CodeAreas>()
        {
            new CodeAreas("111", 1, null)
        };

        // Act
        Action result = () => _sut.IsDiscountCodeApplicable(session, discountCode, codeAreas, codeApplicableAreas);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("The given code area doesn't match the discount code");
    }
}

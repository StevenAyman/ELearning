using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Purchases;
public sealed class PaidCodeTests
{
    private readonly PaidCode _sut;

    public PaidCodeTests()
    {
        _sut = new PaidCode("12", "Anything", new Money(50), DateTime.Now);
    }

    [Fact]
    public void ExpireCode_ShouldThrowException_WhenCodeStatusIsExpired()
    {
        // Arrange
        _sut.ExpireCode(DateTime.Now);

        // Act
        Action result = () => _sut.ExpireCode(DateTime.UtcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Error. This code is already expired.");
    }

    [Fact]
    public void ExpireCode_ShouldThrowException_WhenCodeIsUsed()
    {
        // Arrange
        _sut.UseCode("21", DateTime.UtcNow);

        // Act
        Action result = () => _sut.ExpireCode(DateTime.UtcNow);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Error. This code is used before. you can't expire it.");
    }

    [Fact]
    public void ExpireCode_ShouldMarkCodeStatusAsExpired_WhenCodeIsExpired()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;

        // Act
        _sut.ExpireCode(utcNow);

        // Assert
        _sut.Status.Should().Be(PaidCodeStatus.Expired);
        _sut.ExpiredAtUtc.Should().Be(utcNow);
    }

    [Fact]
    public void UseCode_ShouldMarkCodeStatusAsUsed_WhenCodeIsUsed()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;

        // Act
        _sut.UseCode("12", utcNow);

        // Assert
        _sut.Status.Should().Be(PaidCodeStatus.Used);
        _sut.UsedAtUtc.Should().Be(utcNow);
        _sut.ExpiredAtUtc.Should().Be(null);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Instructors;
public sealed class SessionDomainServiceTests
{
    private readonly SessionDomainService _sut;
    public SessionDomainServiceTests()
    {
        _sut = new SessionDomainService();
    }
    [Fact]
    public void CreateSession_ShouldCreateNewSession_WhenInputsAreValid()
    {
        // Arrange
        string id = $"s_{Guid.CreateVersion7()}";
        var title = new Title("Week 01");
        var description = new Description("First week session");
        var price = new Money(60);
        var createdOnUtc = DateTime.UtcNow;
        var instructorId = $"I_{Guid.CreateVersion7()}";
        var sessionId = $"s_{Guid.CreateVersion7()}";
        var session = new Session(id, title, description, price, SessionStatus.Draft, createdOnUtc, instructorId, sessionId, "");

        // Act
        var result = _sut.CreateSession(id, title, description, price, createdOnUtc, instructorId, sessionId, "");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(session);
    }
}

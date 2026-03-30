using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Users;
using ELearning.Domain.Users.Events;
using FluentAssertions;

namespace ELearning.Domain.Tests.Unit.Users;
public sealed class UserTests
{
    [Fact]
    public void Register_ShouldReturnNewUser_WhenDataIsValid()
    {
        // Arrange
        string id = $"u_{Guid.CreateVersion7()}";
        var firstName = new FirstName("Steven");
        var lastName = new LastName("Ayman");
        var email = new Email("stevenayman@gamil.com");
        var dateOfBirth = Date.Create( new DateOnly(2002, 5, 1) );
        var city = "Giza";
        var joinedDate = DateTime.Now;

        // Act
        var result = User.Register(id, firstName, lastName, email, dateOfBirth, city, joinedDate);

        // Assert
        result.Id.Should().Be(id);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
        result.Email.Should().Be(email);
        result.City.Should().Be(city);
        result.JoinedOnUtc.Should().Be(joinedDate);
    }

    [Fact]
    public void Register_ShouldRaiseUserRegisteredDomainEvent_WhenUserRegisteredSuccessfully()
    {
        // Arrange
        string id = $"u_{Guid.CreateVersion7()}";
        var firstName = new FirstName("Steven");
        var lastName = new LastName("Ayman");
        var email = new Email("stevenayman@gamil.com");
        var dateOfBirth = Date.Create(new DateOnly(2002, 5, 1));
        var city = "Giza";
        var joinedDate = DateTime.Now;

        // Act
        var result = User.Register(id, firstName, lastName, email, dateOfBirth, city, joinedDate);

        // Assert
        result.DomainEvents.Should().HaveCount(1);
        result.DomainEvents.Should().Contain(new UserRegisteredDomainEvent(id));
    }
}

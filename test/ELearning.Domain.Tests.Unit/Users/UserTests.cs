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
    private readonly User _user;
    public UserTests()
    {
        string id = $"u_{Guid.CreateVersion7()}";
        var firstName = new FirstName("Steven");
        var lastName = new LastName("Ayman");
        var email = new Email("stevenayman@gamil.com");
        var dateOfBirth = Date.Create(new DateOnly(2002, 5, 1));
        var city = "Giza";
        var joinedDate = DateTime.Now;
        _user = User.Register(id, firstName, lastName, email, dateOfBirth, city, joinedDate);
    }
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

    [Fact]
    public void UpdateDateOfBirth_ShouldUpdateDateOfBirth_WhenItIsValid()
    {
        // Arrange
        var updatedDateOfBirth = Date.Create(new DateOnly(2005, 1, 1));

        // Act
        var result = _user.UpdateDateOfBirth(updatedDateOfBirth);

        // Assert
        result.DateOfBirth.Should().Be(updatedDateOfBirth);
    }

    [Fact]
    public void UpdateFirstName_ShouldUpdateUserFirstName_WhenItIsValid()
    {
        // Arrange
        var updatedFirstName = new FirstName("Ivan");

        // Act
        var result = _user.UpdateFirstName(updatedFirstName);

        // Assert
        result.FirstName.Should().Be(updatedFirstName);
    }

    [Fact]
    public void UpdateFirstName_ShouldThrowException_WhenItIsInvalid()
    {
        // Arrange
        var updatedFirstName = new FirstName("");

        // Act
        Action result = () => _user.UpdateFirstName(updatedFirstName);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("First name can't be null or empty");
    }

    [Fact]
    public void UpdateLastName_ShouldUpdateUserLastName_WhenItIsValid()
    {
        // Arrange
        var updatedLastName = new LastName("Fayek");

        // Act
        var result = _user.UpdateLastName(updatedLastName);

        // Assert
        result.LastName.Should().Be(updatedLastName);
    }

    [Fact]
    public void UpdateLastName_ShouldThrowException_WhenItIsInvalid()
    {
        // Arrange
        var updatedLastName = new LastName("");

        // Act
        Action result = () => _user.UpdateLastName(updatedLastName);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Last name can't be null or empty");
    }

    [Fact]
    public void UpdateEmail_ShouldUpdateUserEmail_WhenItIsValid()
    {
        // Arrange
        var updatedEmail = new Email("stevenayman0@gmail.com");

        // Act
        var result = _user.UpdateEmail(updatedEmail);

        // Assert
        result.Email.Should().Be(updatedEmail);
    }

    [Fact]
    public void UpdateEmail_ShouldThrowException_WhenItIsInvalid()
    {
        // Arrange
        var updatedEmail = new Email("");

        // Act
        Action result = () => _user.UpdateEmail(updatedEmail);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("Invalid email");
    }

    [Fact]
    public void UpdateCity_ShouldUpdateUserCity_WhenItIsValid()
    {
        // Arrange
        var updatedCity = "Cairo";

        // Act
        var result = _user.UpdateCity(updatedCity);

        // Assert
        result.City.Should().Be(updatedCity);
    }

    [Fact]
    public void UpdateCity_ShouldThrowException_WhenItIsInvalid()
    {
        // Arrange
        var updatedCity = "";

        // Act
        Action result = () => _user.UpdateCity(updatedCity);

        // Assert
        result.Should().Throw<ApplicationException>()
            .WithMessage("City can't be null or empty");
    }
}

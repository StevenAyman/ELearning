using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Domain.Users.Events;

namespace ELearning.Domain.Users;
public sealed class User : BaseEntity
{
    private User() { }
    private User(
        string id,
        FirstName firstName,
        LastName lastName,
        Email email,
        Date dateOfBirth,
        string city,
        string identityId) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        JoinedOnUtc = DateTime.UtcNow;
        City = city;
        IdentityId = identityId;
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public Date DateOfBirth { get; private set; }
    public string City { get; private set; }
    public DateTime JoinedOnUtc { get; private set; }
    public string IdentityId { get; private set; }

    public static User Register(
        string id, 
        FirstName firstName, 
        LastName lastName,
        Email email, 
        Date dateOfBirth, 
        string city, 
        DateTime utcNow, 
        string identityId)
    {
        var user = new User(id, firstName, lastName, email, dateOfBirth, city, identityId);
        user.JoinedOnUtc = utcNow;
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));
        return user;
    }

    public User UpdateDateOfBirth(Date newDateOfBirth)
    {
        DateOfBirth = newDateOfBirth;
        return this;
    }
    public User UpdateFirstName(FirstName firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName.Value))
        {
            throw new ApplicationException("First name can't be null or empty");
        }
        FirstName = firstName;
        return this;
    }
    public User UpdateLastName(LastName lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName.Value))
        {
            throw new ApplicationException("Last name can't be null or empty");
        }

        LastName = lastName;

        return this;

    }
    public User UpdateEmail(Email email)
    {
        if (string.IsNullOrWhiteSpace(email.Value))
        {
            throw new ApplicationException("Invalid email");
        }

        Email = email;
        return this;
    }

    public User UpdateCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ApplicationException("City can't be null or empty");
        }

        City = city;

        return this;
    }
}

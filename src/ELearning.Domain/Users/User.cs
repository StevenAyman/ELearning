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
        string city) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        JoinedOnUtc = DateTime.UtcNow;
        City = city;
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public Date DateOfBirth { get; private set; }
    public string City { get; private set; }
    public DateTime JoinedOnUtc { get; private set; }

    public static User Register(string id, FirstName firstName, LastName lastName, Email email, Date dateOfBirth, string city, DateTime utcNow)
    {
        var user = new User(id, firstName, lastName, email, dateOfBirth, city);
        user.JoinedOnUtc = utcNow;
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));
        return user;
    }
}

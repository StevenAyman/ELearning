using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Users;
public sealed class User : BaseEntity
{
    private User():base() { }
    public User(
        string id,
        FirstName firstName,
        LastName lastName,
        Email email,
        Date dateOfBirth) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        JoinedOnUtc = DateTime.UtcNow;
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public Date DateOfBirth { get; private set; }
    public DateTime JoinedOnUtc { get; private set; }
}

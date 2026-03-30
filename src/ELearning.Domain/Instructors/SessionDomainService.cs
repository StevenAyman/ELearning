using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Students;

namespace ELearning.Domain.Instructors;
public class SessionDomainService
{
    public Session CreateSession(string id, Title title, Description description, Money price, DateTime createdOnUtc, string instructorId)
    {
        return new Session(id, title, description, price, SessionStatus.Draft, createdOnUtc, instructorId);
    }
}

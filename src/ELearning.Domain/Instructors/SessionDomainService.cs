using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Instructors;
public class SessionDomainService
{
    public Session CreateSession(string id, Title title, Description description, Money price, DateTime createdOnUtc, string instructorId, string sessionId, string classId)
    {
        return new Session(id, title, description, price, SessionStatus.Draft, createdOnUtc, instructorId, sessionId, classId);
    }
}

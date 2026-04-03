using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Ratings.Events;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Domain.Ratings;
public sealed class SessionRatingDomainService
{
    private readonly IPurchaseRepository _purchaseRepo;

    public SessionRatingDomainService(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepo = purchaseRepository;
    }

    public async Task<Result<SessionsRating>> RateSession(string sessionId, string studentId, Rating rating, DateTime createdAt, CancellationToken token = default)
    {
        var purchaseSpecs = new BaseSpecifications<Purchase>(p => 
        p.StudentId == studentId && 
        p.SessionId == sessionId && 
        p.PurchasedAtUtc.HasValue);

        var purchase = await _purchaseRepo.GetWithSpecAsync(purchaseSpecs, token);

        if (purchase is null)
        {
            return Result<SessionsRating>.Failure(PurchaseErrors.PurchaseNotFound);
        }

        var sessionRatings = SessionsRating.CreateNewRating(sessionId, studentId, rating, createdAt);

        sessionRatings.RaiseDomainEvent(new SessionRatedDomainEvent(sessionId, rating));

        return Result<SessionsRating>.Succuss(sessionRatings);
    }
}

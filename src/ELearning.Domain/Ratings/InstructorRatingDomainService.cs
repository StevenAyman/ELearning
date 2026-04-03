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
public sealed class InstructorRatingDomainService
{
    private readonly IPurchaseRepository _purchaseRespository;
    private readonly ISessionRepository _sessionRespository;
    private readonly IInstructorsRating _instructorRatingRepo;

    public InstructorRatingDomainService(
        IPurchaseRepository purchaseRepository, 
        ISessionRepository sessionRepository, 
        IInstructorsRating instructorsRatingRepository)
    {
        _purchaseRespository = purchaseRepository;
        _sessionRespository = sessionRepository;
        _instructorRatingRepo = instructorsRatingRepository;
    }

    public async Task<Result<InstructorsRating>> RateInstructor(string instructorId, string studentId, Rating rating, DateTime utcNow, CancellationToken token = default)
    {
        var instructorRatingSpec = new BaseSpecifications<InstructorsRating>(IR => IR.InstructorId == instructorId && IR.StudentId == studentId);

        var instructorStudentRating = await _instructorRatingRepo.GetWithSpecAsync(instructorRatingSpec, token);

        if (instructorStudentRating is not null)
        {
            throw new ApplicationException("You should update the rating not creating new rating");
        }

        var purchaseSpecs = new BaseSpecifications<Purchase>(p => p.StudentId == studentId && p.PurchasedAtUtc.HasValue);
        var studentPurchases = await _purchaseRespository.GetAllWithSpecAsync(purchaseSpecs, token);
        var purchaseSessionIds = studentPurchases.Select(p => p.SessionId).Distinct();


        var sessionSpecs = new BaseSpecifications<Session>(s => purchaseSessionIds.Contains(s.Id));
        var sessions = await _sessionRespository.GetAllWithSpecAsync(sessionSpecs, token);
        if (!sessions.Any())
        {
            return Result<InstructorsRating>.Failure(PurchaseErrors.PurchaseNotFound);
        }

        var sessionWithInstructor = sessions.FirstOrDefault(s => s.InstructorId == instructorId);
        if (sessionWithInstructor is null)
        {
            return Result<InstructorsRating>.Failure(RatingsErrors.InstructorNotTeaching);
        }

        var instructorRating = InstructorsRating.CreateNewRating(instructorId, studentId, rating, utcNow);

        instructorRating.RaiseDomainEvent(new InstructorRatedDomainEvent(instructorId, rating));

        return Result<InstructorsRating>.Succuss(instructorRating);
    }
}



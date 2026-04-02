using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;

namespace ELearning.Domain.Discounts;
public sealed class DiscountDomainService
{
    public bool ValidateDiscountCodeExpiration(
        DiscountCode discountCode,
        DateTime utcNow)
    {
        if (discountCode is null)
        {
            throw new ApplicationException("Invalid discount code");
        }

        if (discountCode.ExpiredAtUtc.HasValue)
        {
            return false;
        }

        if (discountCode.ExpireType == DiscountExpirationType.Period && utcNow >= discountCode.ExpirePeriod?.EndDate ||
            discountCode.ExpireType == DiscountExpirationType.LimitedCount && discountCode.CurrentCount == discountCode.CountLimit)
        {
            discountCode.ExpireCode(utcNow);
            return false;
        }

        return true;
    }

    public bool IsDiscountCodeApplicable(
        Session session, 
        DiscountCode discountCode, 
        IReadOnlyList<CodeAreas> codeAreas,
        IReadOnlyList<CodeApplicableArea> codeApplicableAreas)
    {
        if (session is null)
        {
            throw new ApplicationException("Session can't be null");
        }
        if (codeAreas is null ||
            !codeAreas.Any() ||
            codeApplicableAreas is null ||
            !codeApplicableAreas.Any())
        {
            throw new ApplicationException("The code Areas shouldn't be null");
        }

        foreach (var codeArea in codeAreas)
        {
            if (codeArea is null ||
                codeArea.CodeId != discountCode.Id)
            {
                throw new ApplicationException("The given code area doesn't match the discount code");
            }

            var codeAreaType = codeApplicableAreas.FirstOrDefault(c => c.Id == codeArea.AppplicableAreaId)?.Type;
            if (codeAreaType is null)
            {
                throw new ApplicationException("Something went wrong.");
            }

            if (codeAreaType.Value == DiscountApplicableAreas.General ||
                codeAreaType.Value == DiscountApplicableAreas.Session && codeArea.TargetId == session.Id ||
                codeAreaType.Value == DiscountApplicableAreas.Instructor && codeArea.TargetId == session.InstructorId ||
                codeAreaType.Value == DiscountApplicableAreas.Subject && codeArea.TargetId == session.SubjectId)
            {
                return true;
            }
        }

        return false;
    }

}

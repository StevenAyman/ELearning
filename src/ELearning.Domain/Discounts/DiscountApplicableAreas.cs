namespace ELearning.Domain.Discounts;

/*
 * Discount End  =>  Period , UsageCount  (Done)
 * Discount Type =>  FixedDiscount , PercentageDiscount (Done)
 * Discount      =>  General, Subject, Instuctor, Specific Session
 * 
 */

public static class DiscountApplicableAreas
{
    public static readonly string General = "general";
    public static readonly string Subject = "subject";
    public static readonly string Instructor = "instructor";
    public static readonly string Session = "session";
}

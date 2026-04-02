namespace ELearning.Domain.Discounts;

/*
 * Discount End  =>  Period , UsageCount  (Done)
 * Discount Type =>  FixedDiscount , PercentageDiscount (Done)
 * Discount      =>  General, Subject, Instuctor, Specific Session
 * 
 */

public static class DiscountApplicableAreas
{
    public static readonly string General = "General";
    public static readonly string Subject = "Subject";
    public static readonly string Instructor = "Instructor";
    public static readonly string Session = "Session";
}

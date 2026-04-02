using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Discounts;
public static class DiscountErrors
{
    public static readonly Error InvalidCode = new Error(
        "DiscountCode.Invalid",
        "Discount code shouldn't be null or empty");

    public static readonly Error InvalidExpirationCriteria = new(
        "DiscountCode.ExpirationCriteria",
        "Error. Expiration policy doesn't meet the expiration type.");

    public static readonly Error ExpiredCode = new(
        "DiscountCode.Expired",
        "Error. The current code is already expired");

    public static readonly Error NotFound = new(
        "DiscountCode.NotFound",
        "Error. This code is incorrect.");

    public static readonly Error InvalidExpirePeriod = new(
        "DiscountCode.InvalidExpirePeriod",
        "The expiration period is invalid.");

    public static readonly Error InvalidLimitCount = new(
        "DiscountCode.InvalidLimitCount",
        "Invalid operation you can't set count limit less than or equal current limit.");

    public static readonly Error InvalidDiscountAmount = new(
        "DiscountCode.InvalidDiscountAmount",
        "Discount amount can't be null or less than 0");

    public static readonly Error InvalidPercentageAmount = new(
        "DiscountCode.InvalidPercentageAmount",
        "Invalid discount amount should be between 1% and 100%");

}

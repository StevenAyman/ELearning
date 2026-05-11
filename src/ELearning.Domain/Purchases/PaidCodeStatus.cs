using System.Runtime.Serialization;

namespace ELearning.Domain.Purchases;

public enum PaidCodeStatus
{
    [EnumMember(Value = "available")]
    Available = 1,
    [EnumMember(Value = "used")]
    Used = 2,
    [EnumMember(Value = "expired")]
    Expired = 3,
}

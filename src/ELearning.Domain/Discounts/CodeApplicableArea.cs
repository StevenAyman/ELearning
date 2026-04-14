using ELearning.Domain.Shared;

namespace ELearning.Domain.Discounts;

public sealed class CodeApplicableArea
{
    private CodeApplicableArea() { }
    public CodeApplicableArea(
        TypeName type)
    {
        Type = type;
    }
    public int Id { get; private set; }
    public TypeName Type { get; private set; }
}

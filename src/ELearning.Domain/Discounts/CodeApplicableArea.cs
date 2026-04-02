using ELearning.Domain.Shared;

namespace ELearning.Domain.Discounts;

public sealed class CodeApplicableArea
{
    private CodeApplicableArea() { }
    public CodeApplicableArea(
        int id,
        TypeName type)
    {
        Id = id;
        Type = type;
    }
    public int Id { get; private set; }
    public TypeName Type { get; private set; }
}

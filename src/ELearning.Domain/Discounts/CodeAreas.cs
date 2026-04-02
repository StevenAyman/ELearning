namespace ELearning.Domain.Discounts;

public sealed class CodeAreas
{
    private CodeAreas() { }
    public CodeAreas(string codeId, int appplicableAreaId, string? targetId)
    {
        CodeId = codeId;
        AppplicableAreaId = appplicableAreaId;
        TargetId = targetId;
    }

    public int AppplicableAreaId { get; private set; }
    public string CodeId { get; private set; }
    public string? TargetId { get; private set; }
}

using ELearning.Api.DTOs.Shared;

namespace ELearning.Api.Services;

public sealed class LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
{
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public LinkDto Create(
        string endpointName, 
        string rel,
        string method,
        object? values = null,
        string? controller = null)
    {
        string? href = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext!,
            endpointName,
            controller,
            values);

        return new LinkDto
        {
            Href = href ?? throw new Exception("Href can't be null or empty"),
            Method = method,
            Rel = rel,
        };
    }
}

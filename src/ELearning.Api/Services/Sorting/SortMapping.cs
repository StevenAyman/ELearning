namespace ELearning.Api.Services.Sorting;

public sealed record SortMapping(string SortField, string PropertyName, bool Reverse = false);

public interface ISortMappingDefinition;


public sealed class SortMappingDefinition<TSource, TDestination> : ISortMappingDefinition
{
    public required SortMapping[] Mappings { get; init; }
}

public sealed class SortMappingProvider(IEnumerable<ISortMappingDefinition> sortMappingDefinitions)
{
    public SortMapping[] GetMappings<TSource, TDestination>()
    {
        var sortMapping = sortMappingDefinitions.OfType<SortMappingDefinition<TSource, TDestination>>()
                                                .FirstOrDefault();

        if (sortMapping is null)
        {
            throw new ApplicationException("Sort mapping can't be null");
        }

        return sortMapping.Mappings;
    }
}

public static class SortMappingParser
{
    public static string ParseSort(string sort, SortMapping[] sortMapping, string defaultSort = "id")
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return defaultSort;
        }

        var sortFields = sort.Split(',')
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        var orderParts = new List<string>();
        foreach(var field in sortFields)
        {
            var (sortField, isDescending) = ParseField(field);

            var mapping = sortMapping.First(s => s.SortField.Equals(sortField, StringComparison.OrdinalIgnoreCase));

            string direction = (isDescending, mapping.Reverse) switch
            {
                (true, false) => "DESC",
                (true, true) => "ASC",
                (false, true) => "DESC",
                (false, false) => "ASC"
            };

            orderParts.Add($"{mapping.PropertyName} {direction}");
        }

        return string.Join(',', orderParts);
    }

    private static (string SortField, bool IsDescending) ParseField(string field)
    {
        var fieldParts = field.Split(' ');
        var sortField = fieldParts[0];
        var isDescending = fieldParts.Length > 1 && fieldParts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

        return (sortField, isDescending);
    }
}


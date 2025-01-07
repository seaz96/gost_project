namespace GostStorage.Models.Search;

public class SearchQuery
{
    public string? Text { get; init; }
    public SearchFilters? SearchFilters { get; init; }
    public uint Limit { get; init; }
    public uint Offset { get; init; }
}
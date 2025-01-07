namespace GostStorage.Models.Search;

public class SearchQuery
{
    public string? Text { get; init; }
    public SearchFilters? SearchFilters { get; init; }
    public int Limit { get; init; }
    public int Offset { get; init; }
}
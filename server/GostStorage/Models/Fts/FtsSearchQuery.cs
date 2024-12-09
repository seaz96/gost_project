namespace GostStorage.Models.Docs;

public class FtsSearchQuery
{
    public string? Text { get; init; }
    public FtsSearchFilters? SearchFilters { get; init; }
    public int Limit { get; init; }
    public int Offset { get; init; }
}
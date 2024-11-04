namespace GostStorage.Models.Docs;

public class FtsSearchQuery
{
    public string? Text { get; init; }
    public FtsSearchFilters? SearchFilters { get; init; }
    public int Take { get; init; }
    public int Skip { get; init; }
}
namespace GostStorage.Models.Search;

public class SearchQuery
{
    public string? Text { get; set; }
    public SearchFilters? SearchFilters { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
}
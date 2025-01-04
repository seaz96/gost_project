namespace GostStorage.Models.Statistic;

public class DocumentViewsResponse
{
    public long DocId { get; set; }

    public string? Designation { get; set; }

    public string? FullName { get; set; }

    public int Views { get; set; }
}
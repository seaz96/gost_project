using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public record DocWithStatusModel
{
    public long DocId { get; set; }

    public string? Designation { get; set; }

    public DocStatuses Status { get; set; }
}
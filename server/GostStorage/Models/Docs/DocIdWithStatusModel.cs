using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class DocWithStatusModel
{
    public long DocId { get; set; }

    public string? Designation { get; set; }

    public DocStatuses Status { get; set; }
}
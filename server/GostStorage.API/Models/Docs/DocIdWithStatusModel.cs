using GostStorage.API.Navigations;

namespace GostStorage.API.Models.Docs;

public class DocWithStatusModel
{
    public long DocId { get; set; }

    public string? Designation { get; set; }
    
    public DocStatuses Status { get; set; }
}
using GostStorage.Domain.Navigations;

namespace GostStorage.Domain.Models;

public class DocWithStatusModel
{
    public long DocId { get; set; }

    public string? Designation { get; set; }
    
    public DocStatuses Status { get; set; }
}
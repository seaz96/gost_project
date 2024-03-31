using Gost_Project.Data.Entities.Navigations;

namespace Gost_Project.Data.Models.Docs;

public class DocWithStatusModel
{
    public long DocId { get; set; }

    public string? Designation { get; set; }
    
    public DocStatuses Status { get; set; }
}
using GostStorage.API.Navigations;

namespace GostStorage.API.Models.Stats;

public class GetCountOfDocsModel
{
    public DocStatuses? Status { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
}
using Gost_Project.Data.Entities.Navigations;

namespace Gost_Project.Data.Models;

public class GetCountOfDocsModel
{
    public DocStatuses Status { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}
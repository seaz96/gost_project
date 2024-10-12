using GostStorage.API.Entities.Base;
using GostStorage.API.Navigations;

namespace GostStorage.API.Entities;

public class DocStatisticEntity : BaseEntity
{
    public long DocId { get; set; }
    
    public long UserId { get; set; }
    
    public ActionType Action { get; set; }
    
    public string? OrgBranch { get; set; }

    public DateTime Date { get; set; }
}
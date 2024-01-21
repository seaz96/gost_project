using Gost_Project.Data.Entities.Navigations;

namespace Gost_Project.Data.Entities;

public class DocStatisticEntity : BaseEntity
{
    public long DocId { get; set; }
    
    public long UserId { get; set; }
    
    public ActionType Action { get; set; }
    
    public DateTime Date { get; set; }
}
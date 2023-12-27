namespace Gost_Project.Data.Entities;

public class DocStatisticEntity : BaseEntity
{
    public long DocId { get; set; }
    
    public int Views { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Changed { get; set; }
}
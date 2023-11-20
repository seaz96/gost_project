namespace Gost_Project.Data.Entities;

public class GostEntity : BaseEntity
{
    public long CompanyId { get; set; }
    
    public long ActualFieldId { get; set; }
    
    public long PrimaryFieldId { get; set; }
}
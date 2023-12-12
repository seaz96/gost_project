namespace Gost_Project.Data.Entities;

public class GostEntity : BaseEntity
{
    public required long ActualFieldId { get; set; }
    
    public required long PrimaryFieldId { get; set; }
}
namespace Gost_Project.Data.Entities;

public class NormativeReferenceEntity : BaseEntity
{
    public required long ParentalGostId { get; set; }
    
    public required long ChildGostId { get; set; }
}
namespace Gost_Project.Data.Entities;

public class DocReferenceEntity : BaseEntity
{
    public long ParentalDocId { get; set; }

    public long ChildDocId { get; set; }
}
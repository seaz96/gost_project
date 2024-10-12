using GostStorage.Entities.Base;

namespace GostStorage.Entities;

public class DocReferenceEntity : BaseEntity
{
    public long ParentalDocId { get; set; }

    public long ChildDocId { get; set; }
}
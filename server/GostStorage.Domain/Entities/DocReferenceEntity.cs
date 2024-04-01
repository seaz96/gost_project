using GostStorage.Domain.Entities.Base;

namespace GostStorage.Domain.Entities;

public class DocReferenceEntity : BaseEntity
{
    public long ParentalDocId { get; set; }

    public long ChildDocId { get; set; }
}
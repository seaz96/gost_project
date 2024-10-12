using GostStorage.API.Entities.Base;

namespace GostStorage.API.Entities;

public class DocReferenceEntity : BaseEntity
{
    public long ParentalDocId { get; set; }

    public long ChildDocId { get; set; }
}
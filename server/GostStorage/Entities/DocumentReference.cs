using GostStorage.Entities.Base;

namespace GostStorage.Entities;

public class DocumentReference : BaseEntity
{
    public long ParentalDocId { get; set; }

    public long ChildDocId { get; set; }
}
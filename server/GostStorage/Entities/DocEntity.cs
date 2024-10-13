using GostStorage.Entities.Base;

namespace GostStorage.Entities;

public class DocEntity : BaseEntity
{
    public long? ActualFieldId { get; set; }

    public long PrimaryFieldId { get; set; }
}
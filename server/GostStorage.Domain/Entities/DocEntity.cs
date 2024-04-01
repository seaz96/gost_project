using GostStorage.Domain.Entities.Base;

namespace GostStorage.Domain.Entities;

public class DocEntity : BaseEntity
{
    public long? ActualFieldId { get; set; }

    public long PrimaryFieldId { get; set; }
}
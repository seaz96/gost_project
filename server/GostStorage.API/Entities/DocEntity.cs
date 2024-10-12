using GostStorage.API.Entities.Base;

namespace GostStorage.API.Entities;

public class DocEntity : BaseEntity
{
    public long? ActualFieldId { get; set; }

    public long PrimaryFieldId { get; set; }
}
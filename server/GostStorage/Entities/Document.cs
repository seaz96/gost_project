using System.ComponentModel.DataAnnotations.Schema;
using GostStorage.Entities.Base;

namespace GostStorage.Entities;

public class Document : BaseEntity
{
    [Index]
    public required string Designation { get; set; }
    
    public long ActualFieldId { get; set; }

    public long PrimaryFieldId { get; set; }
}
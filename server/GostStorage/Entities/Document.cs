using System.ComponentModel.DataAnnotations.Schema;
using GostStorage.Entities.Base;
using GostStorage.Navigations;

namespace GostStorage.Entities;

public class Document : BaseEntity
{
    [Index]
    public required string Designation { get; set; }
    
    public DocumentStatus Status { get; set; }
    
    public long ActualFieldId { get; set; }

    public long PrimaryFieldId { get; set; }
}
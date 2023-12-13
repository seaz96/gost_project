using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class GostEntity : BaseEntity
{
    public long ActualFieldId { get; set; }
    
    public long PrimaryFieldId { get; set; }
}
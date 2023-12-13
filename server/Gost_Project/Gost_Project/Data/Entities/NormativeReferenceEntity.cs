using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class NormativeReferenceEntity : BaseEntity
{
    public long ParentalGostId { get; set; }
    
    public long ChildGostId { get; set; }
}
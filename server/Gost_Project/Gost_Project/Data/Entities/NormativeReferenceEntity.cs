using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class NormativeReferenceEntity : BaseEntity
{
    public long ParentalDocId { get; set; }
    
    public long ChildDocId { get; set; }
}
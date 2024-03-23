using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gost_Project.Data.Entities;

public class BaseEntity
{
    [Key]
    [Index(nameof(Id))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
}
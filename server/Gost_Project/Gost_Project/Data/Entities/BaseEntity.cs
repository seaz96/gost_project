using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Gost_Project.Data.Entities;

public class BaseEntity
{
    [Required]
    public long Id { get; set; }
}
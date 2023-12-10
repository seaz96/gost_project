using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Gost_Project.Data.Entities;

public class BaseEntity
{
    public required long Id { get; set; }
}
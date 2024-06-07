using GostStorage.Domain.Entities;

namespace GostStorage.Services.Models.Docs;

public class DocumentESModel
{
    public long Id { get; set; }
    
    public FieldEntity Field { get; set; }
    
    public string Data { get; set; }
}
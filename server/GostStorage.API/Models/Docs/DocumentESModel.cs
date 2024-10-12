using GostStorage.API.Entities;

namespace GostStorage.API.Models.Docs;

public class DocumentESModel
{
    public long Id { get; set; }
    
    public FieldEntity Field { get; set; }
    
    public string Data { get; set; }
}
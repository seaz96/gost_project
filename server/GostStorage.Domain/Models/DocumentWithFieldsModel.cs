using GostStorage.Domain.Entities;

namespace GostStorage.Domain.Models;

public class DocumentWithFieldsModel
{
    public long DocId { get; set; }
    
    public FieldEntity Primary { get; set; }
    
    public FieldEntity Actual { get; set; }

    public List<DocWithStatusModel> References { get; set; }
}
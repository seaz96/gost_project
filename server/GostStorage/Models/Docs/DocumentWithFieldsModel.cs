using GostStorage.Entities;

namespace GostStorage.Models.Docs;

public class DocumentWithFieldsModel
{
    public long DocId { get; set; }
    
    public FieldEntity Primary { get; set; }
    
    public FieldEntity Actual { get; set; }

    public List<DocWithStatusModel> References { get; set; }
}
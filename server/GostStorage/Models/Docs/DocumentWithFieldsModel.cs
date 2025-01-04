using GostStorage.Entities;

namespace GostStorage.Models.Docs;

public class DocumentWithFieldsModel
{
    public long DocId { get; set; }

    public Field Primary { get; set; }

    public Field Actual { get; set; }

    public List<DocWithStatusModel> References { get; set; }
}
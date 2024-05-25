using GostStorage.Domain.Entities;

namespace GostStorage.Services.Models.Docs;

public class GetDocumentResponseModel
{
    public long DocId { get; set; }

    public FieldEntity Primary { get; set; }

    public FieldEntity Actual { get; set; }

    public List<DocWithStatusModel> References { get; set; }
}
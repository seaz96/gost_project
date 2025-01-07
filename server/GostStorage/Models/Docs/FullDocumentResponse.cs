using GostStorage.Entities;
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class FullDocumentResponse
{
    public long DocId { get; set; }

    public DocumentStatus Status { get; set; }

    public FieldResponse Primary { get; set; }

    public FieldResponse Actual { get; set; }

    public List<ReferenceDocumentResponse> References { get; set; }
}
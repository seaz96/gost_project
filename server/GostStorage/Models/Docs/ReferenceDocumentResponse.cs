using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class ReferenceDocumentResponse
{
    public long Id { get; set; }
    public string Designation { get; set; }
    public DocumentStatus Status { get; set; }
}
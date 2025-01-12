using GostStorage.Entities;
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class FullDocument
{
    public long Id { get; set; }

    public DocumentStatus Status { get; set; }

    public Field Primary { get; set; }

    public Field Actual { get; set; }

    public List<Document> References { get; set; }
}
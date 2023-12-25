using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Models;

public class GetDocumentResponseModel
{
    public FieldEntity Primary { get; set; }
    
    public FieldEntity Actual { get; set; }
}
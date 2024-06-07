namespace GostStorage.Services.Models.Docs;

public record ShortInfoDocumentModel
{
    public long Id { get; set; }
    
    public string CodeOKS { get; set; }
    
    public string Designation { get; set; }
    
    public string FullName { get; set; }
    
    public int RelevanceMark { get; set; }
    
    public string? ApplicationArea { get; set; }
}
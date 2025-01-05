namespace GostStorage.Models.Docs;

public record GeneralDocumentInfoModel
{
    public long Id { get; set; }
    public string CodeOks { get; set; }
    public string Designation { get; set; }
    public string FullName { get; set; }
    public int RelevanceMark { get; set; }
}
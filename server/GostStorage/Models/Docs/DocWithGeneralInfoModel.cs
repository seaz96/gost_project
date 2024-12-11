namespace GostStorage.Models.Docs;

public record DocWithGeneralInfoModel
{
    public long Id { get; set; }

    public string? Designation { get; set; }

    public string? CodeOKS { get; set; }

    public string? FullName { get; set; }

    public string? ApplicationArea { get; set; }
}
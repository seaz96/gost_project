using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public record SearchParametersModel
{
    public string? Name { get; set; }

    public string? CodeOKS { get; set; }

    public string? ActivityField { get; set; }

    public int? AcceptanceYear { get; set; }

    public int? CommissionYear { get; set; }

    public string? Author { get; set; }

    public string? AcceptedFirstTimeOrReplaced { get; set; }

    public string? Content { get; set; }

    public string? KeyWords { get; set; }

    public string? KeyPhrases { get; set; }

    public string? ApplicationArea { get; set; }

    public DocAdoptionLevels? AdoptionLevel { get; set; }

    public string? DocumentText { get; set; }

    public string? Changes { get; set; }

    public string? Amendments { get; set; }

    public HarmonizationLevels? Harmonization { get; set; }
}
using System.ComponentModel.DataAnnotations;
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public record AddNewDocumentRequest
{
    public string? Designation { get; set; }

    public string? FullName { get; set; }

    public string? CodeOKS { get; set; }

    public string? ActivityField { get; set; }

    [Range(1000, 9999)] public int? AcceptanceYear { get; set; }

    [Range(1000, 9999)] public int? CommissionYear { get; set; }

    public string? Author { get; set; }

    public string? AcceptedFirstTimeOrReplaced { get; set; }

    public string? Content { get; set; }

    public string? KeyWords { get; set; }

    public string? ApplicationArea { get; set; }

    public DocAdoptionLevels AdoptionLevel { get; set; }

    public string? DocumentText { get; set; }

    public string? Changes { get; set; }

    public string? Amendments { get; set; }

    public DocStatuses Status { get; set; }

    public HarmonizationLevels Harmonization { get; set; }

    public bool IsPrimary { get; set; }

    public List<string> References { get; set; }
}
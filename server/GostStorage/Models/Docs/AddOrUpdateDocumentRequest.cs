using System.ComponentModel.DataAnnotations;
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public record AddOrUpdateDocumentRequest
{
    public required string Designation { get; init; }

    public string? FullName { get; init; }

    public string? CodeOks { get; init; }

    public string? ActivityField { get; init; }

    [Range(1000, 9999)] public int? AcceptanceYear { get; init; }

    [Range(1000, 9999)] public int? CommissionYear { get; init; }

    public string? Author { get; init; }

    public string? AcceptedFirstTimeOrReplaced { get; init; }

    public string? Content { get; init; }

    public string? KeyWords { get; init; }

    public string? ApplicationArea { get; init; }

    public AdoptionLevel? AdoptionLevel { get; init; }

    public string? DocumentText { get; init; }

    public string? Changes { get; init; }

    public string? Amendments { get; init; }

    public DocumentStatus? Status { get; init; }

    public HarmonizationLevel? Harmonization { get; init; }
    
    public IFormFile? File { get; set; }

    public List<string>? References { get; init; }
}
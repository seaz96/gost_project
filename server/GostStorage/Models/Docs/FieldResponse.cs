using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class FieldResponse
{
    public string Designation { get; set; }

    public string? FullName { get; set; }

    public string? CodeOks { get; set; }

    public string? ActivityField { get; set; }

    public int? AcceptanceYear { get; set; }

    public int? CommissionYear { get; set; }   

    public string? Author { get; set; }

    public string? AcceptedFirstTimeOrReplaced { get; set; }

    public string? Content { get; set; }

    public string? KeyWords { get; set; }

    public string? ApplicationArea { get; set; }

    public AdoptionLevel? AdoptionLevel { get; set; }

    public string? DocumentText { get; set; }

    public string? Changes { get; set; }

    public string? Amendments { get; set; }

    public HarmonizationLevel? Harmonization { get; set; }
    
    public DateTime? LastEditTime { get; set; }
}
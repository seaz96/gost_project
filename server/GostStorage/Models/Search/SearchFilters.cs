using System.ComponentModel.DataAnnotations;
using GostStorage.Navigations;

namespace GostStorage.Models.Search;

public class SearchFilters
{
    public string? CodeOks { get; set; }
    [Range(1000, 9999)] public int? AcceptanceYear { get; set; }
    [Range(1000, 9999)] public int? CommissionYear { get; set; }
    public string? Author { get; set; }
    public string? AcceptedFirstTimeOrReplaced { get; set; }
    public string? KeyWords { get; set; }
    public HashSet<AdoptionLevel>? AdoptionLevel { get; set; }
    public HashSet<DocumentStatus>? Status { get; set; }
    public HashSet<HarmonizationLevel>? Harmonization { get; set; }
    public string? Changes { get; set; }
    public string? Amendments { get; set; }
}
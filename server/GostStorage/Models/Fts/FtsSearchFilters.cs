using System.ComponentModel.DataAnnotations;
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class FtsSearchFilters
{
    public string? CodeOks { get; set; }
    [Range(1000, 9999)] public int? AcceptanceYear { get; set; }
    [Range(1000, 9999)] public int? CommissionYear { get; set; }
    public string? Author { get; set; }
    public string? AcceptedFirstTimeOrReplaced { get; set; }
    public string? KeyWords { get; set; }
    public DocAdoptionLevels? AdoptionLevel { get; set; }
    public DocStatuses Status { get; set; }
    public HarmonizationLevels? Harmonization { get; set; }
}
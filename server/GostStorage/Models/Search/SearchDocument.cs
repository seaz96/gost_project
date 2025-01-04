using System.ComponentModel.DataAnnotations;
using GostStorage.Navigations;

namespace GostStorage.Models.Search;

public class SearchDocument
{
    public long Id { get; set; }
    public string? Designation { get; set; }
    public string? FullName { get; set; }
    public string? CodeOks { get; set; }
    public string? ActivityField { get; set; }
    [Range(1000, 9999)] public int? AcceptanceYear { get; set; }
    [Range(1000, 9999)] public int? CommissionYear { get; set; }
    public string? Author { get; set; }
    public string? AcceptedFirstTimeOrReplaced { get; set; }
    public string? Content { get; set; }
    public string? KeyWords { get; set; }
    public string? ApplicationArea { get; set; }
    public AdoptionLevel? AdoptionLevel { get; set; }
    public string? Changes { get; set; }
    public string? Amendments { get; set; }
    public DocumentStatus Status { get; set; }
    public HarmonizationLevel? Harmonization { get; set; }
    public int IndexedWordsCount { get; set; }
}
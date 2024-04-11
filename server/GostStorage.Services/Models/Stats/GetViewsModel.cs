namespace GostStorage.Services.Models.Stats;

public class GetViewsModel
{
    public string? Designation { get; set; }

    public string? CodeOKS { get; set; }

    public string? ActivityField { get; set; }

    public string? OrgBranch { get; set; }
    
    public DateTime? StartYear { get; set; }
    
    public DateTime? EndYear { get; set; }
}
namespace GostStorage.Models.Statistic;

public class GetViewsModel
{
    public string? Designation { get; set; }

    public string? CodeOks { get; set; }

    public string? ActivityField { get; set; }

    public string? OrgBranch { get; set; }

    public DateTime? StartYear { get; set; }

    public DateTime? EndYear { get; set; }
}
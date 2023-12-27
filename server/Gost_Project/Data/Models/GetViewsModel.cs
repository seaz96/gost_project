namespace Gost_Project.Data.Models;

public class GetViewsModel
{
    public string? Designation { get; set; }

    public string? FullName { get; set; }

    public string? CodeOKS { get; set; }

    public string? ActivityField { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}
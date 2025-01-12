using GostStorage.Navigations;

namespace GostStorage.Models.Statistic;

public class UserActionDocumentModel
{
    public string Designation { get; set; }
    
    public string FullName { get; set; }
    
    public long DocumentId { get; set; }

    public long UserId { get; set; }

    public ActionType Action { get; set; }

    public string? OrgBranch { get; set; }

    public DateTime Date { get; set; }
}
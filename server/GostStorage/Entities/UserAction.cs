using GostStorage.Entities.Base;
using GostStorage.Navigations;

namespace GostStorage.Entities;

public class UserAction : BaseEntity
{
    public long DocId { get; set; }

    public long UserId { get; set; }

    public ActionType Type { get; set; }

    public string? OrgBranch { get; set; }

    public DateTime Date { get; set; }
}
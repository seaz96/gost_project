using GostStorage.Navigations;

namespace GostStorage.Models.Statistic;

public class DocumentCountRequest
{
    public DocumentStatus Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
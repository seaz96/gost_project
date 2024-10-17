using GostStorage.Navigations;

namespace GostStorage.Models.Stats;

public class GetCountOfDocsModel
{
    public DocStatuses? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
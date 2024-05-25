using GostStorage.Domain.Navigations;

namespace GostStorage.Services.Models.Stats;

public class GetCountOfDocsModel
{
    public DocStatuses? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public record ChangeStatusRequest
{
    public long Id { get; set; }

    public DocStatuses Status { get; set; }
}
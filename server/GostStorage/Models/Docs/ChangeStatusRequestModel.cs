using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class ChangeStatusRequestModel
{
    public long Id { get; set; }

    public DocStatuses Status { get; set; }
}
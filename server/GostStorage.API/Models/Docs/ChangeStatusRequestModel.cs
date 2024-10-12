using GostStorage.API.Navigations;

namespace GostStorage.API.Models.Docs;

public class ChangeStatusRequestModel
{
    public long Id { get; set; }

    public DocStatuses Status { get; set; }
}
using GostStorage.Domain.Navigations;

namespace GostStorage.Services.Models.Docs;

public class ChangeStatusRequestModel
{
    public long Id { get; set; }

    public DocStatuses Status { get; set; }
}
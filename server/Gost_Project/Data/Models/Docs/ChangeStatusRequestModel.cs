using Gost_Project.Data.Entities.Navigations;

namespace Gost_Project.Data.Models.Docs;

public class ChangeStatusRequestModel
{
    public long Id { get; set; }

    public DocStatuses Status { get; set; }
}
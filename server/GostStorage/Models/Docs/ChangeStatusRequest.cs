using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public class ChangeStatusRequest
{
    public long Id { get; set; }

    public DocumentStatus Status { get; set; }
}
using GostStorage.Navigations;

namespace GostStorage.Models.Docs;

public record ChangeStatusRequest(long Id, DocumentStatus Status);
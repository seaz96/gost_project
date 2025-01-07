namespace GostStorage.Models.Docs;

public record AddDocumentWithFileRequest : AddDocumentRequest
{
    public IFormFile File { get; set; }
}
namespace GostStorage.Models.Docs;

public record UploadFileRequest
{
    public IFormFile File { get; set; }

    public string Extension { get; set; }
}
namespace GostStorage.Models.Docs;

public record UploadFileModel
{
    public IFormFile File { get; set; }
    
    public string Extension { get; set; }
}
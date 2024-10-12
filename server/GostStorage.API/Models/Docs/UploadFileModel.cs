namespace GostStorage.API.Models.Docs;

public record UploadFileModel
{
    public IFormFile File { get; set; }
    
    public string Extension { get; set; }
}
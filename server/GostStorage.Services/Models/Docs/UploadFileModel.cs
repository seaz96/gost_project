using Microsoft.AspNetCore.Http;

namespace GostStorage.Services.Models.Docs;

public record UploadFileModel
{
    public IFormFile File { get; set; }
    
    public string Extension { get; set; }
}
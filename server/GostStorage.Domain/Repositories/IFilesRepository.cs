using Microsoft.AspNetCore.Http;

namespace GostStorage.Domain.Repositories;

public interface IFilesRepository
{
    public Task UploadFileAsync(IFormFile file, string extension, long docId); 
}
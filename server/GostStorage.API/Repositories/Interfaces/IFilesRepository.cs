namespace GostStorage.API.Repositories.Interfaces;

public interface IFilesRepository
{
    public Task UploadFileAsync(IFormFile file, string extension, long docId); 
}
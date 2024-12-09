namespace GostStorage.Repositories;

public interface IFilesRepository
{
    public Task UploadFileAsync(IFormFile file, string extension, long docId);
}
namespace GostStorage.Repositories.Abstract;

public interface IFilesRepository
{
    public Task UploadFileAsync(IFormFile file, string extension, long docId);
}
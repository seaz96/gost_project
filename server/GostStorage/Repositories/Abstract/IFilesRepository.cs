namespace GostStorage.Repositories.Abstract;

public interface IFilesRepository
{
    public Task<string?> UploadFileAsync(IFormFile file, long docId);
}
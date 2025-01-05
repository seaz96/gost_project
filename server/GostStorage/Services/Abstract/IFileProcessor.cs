using GostStorage.Models.Docs;

namespace GostStorage.Services.Abstract;

public interface IFileProcessor
{
    Task<string> ExtractFileTextSafeAsync(UploadFileModel document);
}
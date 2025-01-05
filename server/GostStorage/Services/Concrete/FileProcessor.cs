using GostStorage.Models.Docs;
using GostStorage.Services.Abstract;
using Serilog;
using Toxy;

namespace GostStorage.Services.Concrete;

public class FileProcessor : IFileProcessor
{
    public async Task<string> ExtractFileTextSafeAsync(UploadFileModel document)
    {
        var path = Path.GetTempPath() + Guid.NewGuid() + document.File.FileName;
        try
        {
            await using var stream = new FileStream(
                path,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                4096,
                FileOptions.DeleteOnClose);
            await document.File.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var text = ParserFactory.CreateText(new ParserContext(path)).Parse();
            return text;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e.Message);
            return null;
            //TODO(azanov.n): сюда надо прикрутить OCR на tesseract для pdf
        }
        finally
        {
            File.Delete(path);
        }
    }
}
using GostStorage.Models.Docs;
using GostStorage.Services.Abstract;
using TikaOnDotNet.TextExtraction;

namespace GostStorage.Services.Concrete;

public class FileProcessor(TextExtractor textExtractor, ILogger logger) : IFileProcessor
{
    public async Task<string> ExtractFileTextSafeAsync(UploadFileModel document)
    {
        try
        {
            return await ProcessFileAsync(document);

        }
        catch (TextExtractionException e)
        {
            logger.LogError(e.Message);
            return null;
            //TODO(azanov.n): сюда надо прикрутить OCR на tesseract для pdf
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }
    
    private async Task<string> ProcessFileAsync(UploadFileModel document)
    {
        using var fileStream = new MemoryStream();
        await document.File.CopyToAsync(fileStream);
        var result = textExtractor.Extract(fileStream.ToArray());
        return result.Text;
    }
}
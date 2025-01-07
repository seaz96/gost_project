using GostStorage.Repositories.Abstract;
using Minio;
using Minio.DataModel.Args;
using IMinioClientFactory = Minio.AspNetCore.IMinioClientFactory;

namespace GostStorage.Repositories.Concrete;

public class FilesRepository(
        IPrimaryFieldsRepository primaryFieldsRepository,
        IMinioClientFactory minioClientFactory,
        IConfiguration configuration)
    : IFilesRepository
{
    private readonly IMinioClient _minioClient = minioClientFactory.CreateClient();

    public async Task<string?> UploadFileAsync(IFormFile file, long docId)
    {
        var primary = (await primaryFieldsRepository.GetFieldsByDocIds(new List<long> { docId })).First();
        var extension = Path.GetExtension(file.FileName);
        var filename = primary.Designation + extension;
        if (file.Length <= 0) return null;
        
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(configuration.GetValue<string>("MINIO_BUCKET"))
            .WithObject(filename)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType("application/octet-stream");

        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

        return filename;
    }
}
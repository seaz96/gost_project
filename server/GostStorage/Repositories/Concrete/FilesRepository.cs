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

    public async Task UploadFileAsync(IFormFile file, string extension, long docId)
    {
        var primary = (await primaryFieldsRepository.GetFieldsByDocIds(new List<long> { docId })).First();
        if (file.Length > 0)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(configuration.GetValue<string>("MINIO_BUCKET"))
                .WithObject(primary.Designation + '.' + extension)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream");

            _ = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
    }
}